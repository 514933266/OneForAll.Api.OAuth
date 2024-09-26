using AutoMapper;
using OAuth.Domain.AggregateRoots;
using OAuth.Domain.Aggregates;
using OAuth.Domain.Enums;
using OAuth.Domain.Interfaces;
using OAuth.Domain.Models;
using OAuth.Domain.Repositorys;
using OAuth.Domain.ValueObjects;
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
        private readonly OAuthLoginSettingVo _setting;
        private readonly IMapper _mapper;
        private readonly IWxmpHttpService _httpService;
        private readonly IIdentityServer4HttpService _identityHttpService;
        private readonly ISysUserRepository _userRepository;
        private readonly ISysWxUserRepository _wxUserRepository;
        private readonly ISysWxClientRepository _clientRepository;

        public WxmpLoginManager(
            OAuthLoginSettingVo setting,
            IMapper mapper,
            IWxmpHttpService httpService,
            IIdentityServer4HttpService identityHttpService,
            ISysUserRepository userRepository,
            ISysWxUserRepository wxUserRepository,
            ISysWxClientRepository clientRepository)
        {
            _setting = setting;
            _mapper = mapper;
            _httpService = httpService;
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
            var msg = await VerifyClientAsync(form);
            if (!msg.Status)
                return msg;

            var client = msg.GetData<SysWxClientAggr>();
            msg = await WxLoginAsync(form, client);
            if (!msg.Status)
                return msg;

            var wxUser = msg.GetData<SysWxUser>();
            var user = await _userRepository.GetAsync(w => w.Mobile == wxUser.Mobile);

            if (user == null && client.SysClient.AutoCreateAccount)
                user = await RegisterAsync(wxUser);

            if (user != null)
            {
                var data = await _wxUserRepository.GetAsync(w => w.AppId == client.AppId && w.SysUserId == user.Id);
                if (data == null)
                {
                    wxUser.SysUserId = user.Id;
                    await _wxUserRepository.AddAsync(wxUser);
                }
                return await OAuth2LoginAsync(user, form.Client);
            }
            else
            {
                return msg.Fail("账号不存在");
            }
        }

        // 校验登录客户端
        private async Task<BaseMessage> VerifyClientAsync(WxmpUserLoginForm form)
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
            // 如果没有该手机号的账号，则使用手机号作为用户名
            var tt = TimeHelper.ToTimeStamp().ToString();
            var useMobile = (await _userRepository.CountAsync(w => w.UserName == wxUser.Mobile)) == 0;
            var user = new SysUser()
            {
                UserName = useMobile ? wxUser.Mobile : "Wxmp".Append(tt),
                Mobile = wxUser.Mobile,
                Name = wxUser.NickName.IsNullOrEmpty() ? "微信用户".Append(tt) : wxUser.NickName,
                Password = StringHelper.GetRandomString(20).ToMd5(),
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
            wxUser.NickName = "微信用户wxmp" + StringHelper.GetRandomString(5);
            wxUser.AccessToken = client.AccessToken;
            wxUser.AccessTokenCreateTime = client.AccessTokenCreateTime;
            wxUser.AccessTokenExpiresIn = client.AccessTokenExpiresIn;

            if (!form.MobileCode.IsNullOrEmpty())
            {
                // 微信手机号登录和快捷登录的code不同，只能用特殊的code换手机号
                var phone = await _httpService.GetPhoneNumberAsync(new WxmpPhoneNumberRequest() { Code = form.MobileCode }, client.AccessToken);
                if (phone.IsNullOrEmpty())
                    return msg.Fail(BaseErrType.DataError, "微信手机号获取失败");
                wxUser.Mobile = phone;
            }
            else if (!form.PhoneNumber.IsNullOrEmpty())
            {
                // 手机验证码登录
                if (!form.PhoneNumber.IsMobile())
                    return msg.Fail(BaseErrType.DataError, "手机号码格式错误");
                wxUser.Mobile = form.PhoneNumber;
            }

            if (!wxUser.Mobile.IsNullOrEmpty())
            {
                msg.Data = wxUser;
                return msg.Success("登录成功");
            }
            else
            {
                return msg.Success("登录失败");
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
                Password = user.Password,
                CaptchaKey = _setting.IgnoreCaptchaKey
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

