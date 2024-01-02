using AutoMapper;
using OAuth.Domain.AggregateRoots;
using OAuth.Domain.Aggregates;
using OAuth.Domain.Enums;
using OAuth.Domain.Interfaces;
using OAuth.Domain.Models;
using OAuth.Domain.Repositorys;
using OAuth.HttpService.Interfaces;
using OAuth.HttpService.Models;
using OneForAll.Core;
using OneForAll.Core.DDD;
using OneForAll.Core.Extension;
using OneForAll.Core.Security;
using OneForAll.Core.Utility;
using SysLog.HttpService.Interfaces;
using SysLog.HttpService.Models;
using System;
using System.Threading.Tasks;

namespace OAuth.Domain
{
    /// <summary>
    /// 微信小程序登录
    /// </summary>
    public class WxmpLoginManager : BaseManager, IWxmpLoginManager
    {
        private readonly IMapper _mapper;
        private readonly IWxmpHttpService _httpService;
        private readonly IWxHttpService _wxHttpService;
        private readonly IIdentityServer4HttpService _identityHttpService;
        private readonly ISysUserRepository _userRepository;
        private readonly ISysWxUserRepository _wxUserRepository;
        private readonly ISysWxClientRepository _clientRepository;

        public WxmpLoginManager(
            IMapper mapper,
            IWxmpHttpService httpService,
            IWxHttpService wxHttpService,
            IIdentityServer4HttpService identityHttpService,
            ISysUserRepository userRepository,
            ISysWxUserRepository wxUserRepository,
            ISysWxClientRepository clientRepository)
        {
            _mapper = mapper;
            _httpService = httpService;
            _wxHttpService = wxHttpService;
            _identityHttpService = identityHttpService;
            _userRepository = userRepository;
            _wxUserRepository = wxUserRepository;
            _clientRepository = clientRepository;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>登录结果</returns>
        public async Task<BaseMessage> LoginAsync(WxmpUserLoginForm form)
        {
            var msg = await BeforeLoginAsync(form);
            if (!msg.Status) return msg;

            var client = msg.GetData<SysWxClientAggr>();
            msg = await WxLoginAsync(form, client);
            if (!msg.Status) return msg;

            var wxUser = msg.GetData<SysWxUser>();
            var user = await _userRepository.GetAsync(w => w.UserName == wxUser.Mobile || w.UserName == wxUser.OpenId || w.Mobile == wxUser.Mobile);
            if (user == null)
            {
                if (!client.SysClient.AutoCreateAccount)
                {
                    msg.Data = null;
                    return msg.Fail("账号不存在");
                }

                user = await RegisterAsync(wxUser);
            }

            if (user != null)
            {
                var data = await _wxUserRepository.GetAsync(w => w.AppId == client.AppId && w.SysUserId == user.Id);
                if (data == null)
                {
                    wxUser.SysUserId = user.Id;
                    var effected = await _wxUserRepository.AddAsync(wxUser);
                    if (effected < 1)
                        return msg.Fail("微信账号关联失败,请稍后再试");
                }
                return await OAuth2LoginAsync(user, form.Client);
            }
            return msg.Fail("登录失败");
        }

        // 登陆前
        private async Task<BaseMessage> BeforeLoginAsync(WxmpUserLoginForm form)
        {
            var msg = new BaseMessage();
            var client = await _clientRepository.GetByClientIdAsync(form.Client.Id);
            if (client == null)
                return msg.Fail(BaseErrType.NotAllow, "客户端未授权");
            if (client.SysClient?.ClientSecret != form.Client.Secret)
                return msg.Fail(BaseErrType.PasswordInvalid, "客户端密码错误");

            msg.Data = client;
            return msg.Success();
        }

        // 注册账号
        private async Task<SysUser> RegisterAsync(SysWxUser wxUser)
        {
            var user = new SysUser()
            {
                UserName = wxUser.Mobile,
                Mobile = wxUser.Mobile,
                Password = StringHelper.GetRandomString(20).ToMd5(),
                Name = "小程序手机用户",
                Status = SysUserStatusEnum.Normal
            };

            var effected = await _userRepository.AddAsync(user);
            if (effected > 0)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        // 微信登陆
        private async Task<BaseMessage> WxLoginAsync(WxmpUserLoginForm form, SysWxClient client)
        {
            var msg = await _httpService.LoginAsync(client.AppId, client.AppSecret, form.Code);
            if (!msg.Status)
                return msg;

            var wxResponse = msg.GetData<WxmpLogin2SessionResponse>();
            var wxUser = _mapper.Map<WxmpLogin2SessionResponse, SysWxUser>(wxResponse);
            wxUser.AppId = client.AppId;
            wxUser.AvatarUrl = form.UserInfo?.AvatarUrl ?? "";
            wxUser.NickName = form.UserInfo?.NickName ?? "微信用户";

            if (!form.MobileCode.IsNullOrEmpty())
            {
                // 微信手机号登录和快捷登录的code不同，只能用特殊的code换手机号
                var tokenResp = await _wxHttpService.GetAccessTokenAsync(client.AppId, client.AppSecret);
                var phone = await _httpService.GetPhoneNumberAsync(new WxmpPhoneNumberRequest() { Code = form.MobileCode }, tokenResp.AccessToken);
                wxUser.Mobile = phone;
                wxUser.AccessToken = tokenResp.AccessToken;
                wxUser.AccessTokenCreateTime = DateTime.Now;
                wxUser.AccessTokenExpiresIn = tokenResp.ExpiresIn;

                msg.Data = wxUser;
                return msg.Success();
            }
            else
            {
                // 如果使用微信快捷登录，没有手机号，则需要先通过手机号和微信号的关联绑定
                return msg.Fail("请通过手机号登录");
            }
        }

        // 授权登录
        private async Task<BaseMessage> OAuth2LoginAsync(SysUser user, LoginClientForm client)
        {
            var msg = new BaseMessage();
            var tokenResponse = await _identityHttpService.LoginAsync(new IdentityServer4Request()
            {
                ClientId = client.Id,
                ClientSecret = client.Secret,
                GrantType = "password",
                Username = user.UserName,
                Password = user.Password
            });

            if (!tokenResponse.AccessToken.IsNullOrEmpty())
            {
                msg.Status = true;
                msg.Data = tokenResponse;
            }
            else if (!tokenResponse.Error.IsNullOrEmpty())
            {
                msg.ErrType = BaseErrType.Fail;
                msg.Status = false;
                msg.Data = null;
                msg.Message = "授权登录失败：" + tokenResponse.Error;
            }
            else
            {
                msg = tokenResponse.Result as BaseMessage;
            }
            return msg;
        }
    }
}

