using OAuth.Domain.AggregateRoots;
using OAuth.Domain.Enums;
using OAuth.Domain.Interfaces;
using OAuth.Domain.Models;
using OAuth.Domain.Repositorys;
using OneForAll.Core;
using OneForAll.Core.DDD;
using OneForAll.Core.Extension;
using OneForAll.Core.Security;
using OneForAll.Core.Utility;
using OneForAll.EFCore;
using SysLog.HttpService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SysLog.HttpService.Interfaces;
using OAuth.Domain.ValueObjects;

namespace OAuth.Domain
{
    /// <summary>
    /// 手机号登录
    /// </summary>
    public class MobileLoginManager : BaseManager, IMobileLoginManager
    {
        private readonly OAuthLoginSettingVo _setting;
        private readonly IIdentityServer4HttpService _identityHttpService;
        private readonly ISysUserRepository _userRepository;
        private readonly ISysClientRepository _clientRepository;

        public MobileLoginManager(
            OAuthLoginSettingVo setting,
            IIdentityServer4HttpService identityHttpService,
            ISysUserRepository userRepository,
            ISysClientRepository clientRepository)
        {
            _setting = setting;
            _identityHttpService = identityHttpService;
            _userRepository = userRepository;
            _clientRepository = clientRepository;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        public async Task<BaseMessage> LoginAsync(MobileLoginForm form)
        {
            var msg = await BeforeLoginAsync(form);
            if (!msg.Status) return msg;

            var client = msg.GetData<SysClient>();

            var user = await _userRepository.GetAsync(w => w.Mobile == form.PhoneNumber);
            if (user == null && client.AutoCreateAccount)
                user = await RegisterAsync(form.PhoneNumber);

            if (user != null)
            {
                return await OAuth2LoginAsync(user, form.Client);
            }
            else
            {
                return msg.Fail("账号不存在");
            }
        }

        // 登陆前
        private async Task<BaseMessage> BeforeLoginAsync(MobileLoginForm form)
        {
            var msg = new BaseMessage();
            var client = await _clientRepository.GetAsync(w => w.ClientId == form.Client.Id);
            if (client == null)
                return msg.Fail(BaseErrType.NotAllow, "客户端未授权");
            if (client.ClientSecret != form.Client.Secret)
                return msg.Fail(BaseErrType.PasswordInvalid, "客户端密码错误");
            if (!form.PhoneNumber.IsMobile())
                return msg.Fail(BaseErrType.DataError, "手机号码格式错误");
            msg.Data = client;
            return msg.Success();
        }

        // 注册账号
        private async Task<SysUser> RegisterAsync(string mobile)
        {
            // 如果没有该手机号的账号，则使用手机号作为用户名
            var tt = TimeHelper.ToTimeStamp().ToString();
            var useMobile = (await _userRepository.CountAsync(w => w.UserName == mobile)) == 0;
            var user = new SysUser()
            {
                UserName = useMobile ? mobile : "m".Append(tt),
                Mobile = mobile,
                Password = StringHelper.GetRandomString(20).ToMd5(),
                Name = "手机用户".Append(tt),
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
