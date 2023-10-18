using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OAuth.Application.Interfaces;
using OAuth.Domain.AggregateRoots;
using OAuth.Domain.Enums;
using OAuth.Domain.Interfaces;
using OAuth.Domain.Models;
using OAuth.Domain.Repositorys;
using OAuth.Domain.ValueObjects;
using OAuth.HttpService;
using OAuth.HttpService.Interfaces;
using OAuth.HttpService.Models;
using OAuth.Public.Models;
using OneForAll.Core;
using OneForAll.Core.Extension;
using OneForAll.Core.Security;
using SysLog.HttpService;
using SysLog.HttpService.Interfaces;
using SysLog.HttpService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Application
{
    /// <summary>
    /// 微信小程序
    /// </summary>
    public class WxmpLoginService : IWxmpLoginService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IWxmpHttpService _httpService;
        private readonly IWxHttpService _wxHttpService;
        private readonly IIdentityServer4HttpService _identityHttpService;
        private readonly ISysWechatUserManager _manager;
        private readonly ISysUserRepository _userRepository;
        private readonly ISysWxClientSettingRepository _clientRepository;

        public WxmpLoginService(
            IMapper mapper,
            IConfiguration config,
            IWxmpHttpService httpService,
            IWxHttpService wxHttpService,
            IIdentityServer4HttpService identityHttpService,
            ISysWechatUserManager manager,
            ISysUserRepository userRepository,
            ISysWxClientSettingRepository clientRepository)
        {
            _mapper = mapper;
            _config = config;
            _manager = manager;
            _httpService = httpService;
            _wxHttpService = wxHttpService;
            _identityHttpService = identityHttpService;
            _userRepository = userRepository;
            _clientRepository = clientRepository;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>登录结果</returns>
        public async Task<BaseMessage> LoginAsync(WxmpUserLoginForm form)
        {
            var client = await _clientRepository.GetAsync(w => w.ClientId == form.Client.Id);
            if (client == null)
                return new BaseMessage().Fail("客户端未授权");

            var msg = await _httpService.LoginAsync(client.AppId, client.AppSecret, form.Code);
            if (msg.Status)
            {

                var wxResponse = msg.GetData<WxmpLogin2SessionResponse>();
                var wxUser = _mapper.Map<WxmpLogin2SessionResponse, SysWechatUser>(wxResponse);
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
                }
                else
                {
                    // 如果使用微信快捷登录，没有手机号，则需要先通过手机号和微信号的关联绑定
                    return msg.Fail("请通过手机号登录");
                }

                var user = await _userRepository.GetAsync(w => w.UserName == wxUser.Mobile || w.UserName == wxUser.OpenId);
                if (!client.AutoCreateAccount && user == null)
                    return new BaseMessage().Fail("请先注册账号");

                if (user?.UserName == wxUser.OpenId)
                {
                    // 有部分旧用户是用OpenId作为账号，此处特殊处理将账号更改为手机号
                    user.UserName = wxUser.Mobile;
                    var effected = await _userRepository.UpdateAsync(user);
                    if (effected < 0)
                        return msg.Fail("登录异常");
                }

                return await LoginAsync(wxUser, user, form.Client);
            };
            return msg;
        }

        // 登录获取token
        private async Task<BaseMessage> LoginAsync(SysWechatUser wxUser, SysUser user, LoginClientForm client)
        {
            var msg = new BaseMessage();
            var reGetUser = user == null ? true : false;
            msg.ErrType = await _manager.LoginAsync(wxUser, user, client.Role);
            if (msg.ErrType == BaseErrType.Success)
            {
                // http调用IdentityServer4登录服务获取access_token
                if (reGetUser)
                    user = await _userRepository.GetAsync(w => w.UserName == wxUser.Mobile);
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
                    msg.Message = "跳转登录失败：" + tokenResponse.Error;
                }
                else
                {
                    msg = tokenResponse.Result as BaseMessage;
                }
            }
            else
            {
                msg.Data = null;
                msg.Message = "跳转登录失败";
            }
            return msg;
        }
    }
}
