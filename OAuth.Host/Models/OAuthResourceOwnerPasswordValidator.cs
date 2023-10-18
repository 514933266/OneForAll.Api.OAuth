using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OneForAll.Core;
using IdentityModel;
using OAuth.Domain.Models;
using OAuth.Domain.ValueObjects;
using OneForAll.Core.Extension;
using OAuth.Host.Models;
using OAuth.Domain.Interfaces;
using System.Linq;
using OAuth.Public.Models;
using IdentityServer4.EntityFramework.Entities;
using OAuth.Domain.AggregateRoots;
using Microsoft.Extensions.Configuration;
using OAuth.Domain.Repositorys;

namespace OAuth.Host.Models
{
    public class OAuthResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly OAuthProviderResource _config;
        private readonly IOAuthLoginManager _loginManager;
        private readonly ISysWechatUserManager _wxUserManager;
        private readonly ISysWxClientSettingRepository _wxClientRepository;
        private readonly IHttpContextAccessor _accessor;
        public OAuthResourceOwnerPasswordValidator(
            OAuthProviderResource config,
            IOAuthLoginManager loginManager,
            ISysWechatUserManager wxUserManager,
            ISysWxClientSettingRepository wxClientRepository,
            IHttpContextAccessor accessor)
        {
            _config = config;
            _accessor = accessor;
            _loginManager = loginManager;
            _wxUserManager = wxUserManager;
            _wxClientRepository = wxClientRepository;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var user = new OAuthLogin()
            {
                UserName = context.UserName,
                Password = context.Password,
                IPAddress = ip
            };
            var result = await _loginManager.LoginAsync(user);
            if (result.ErrType == BaseErrType.Success)
            {
                await ValidateSuccess(context, result);
            }
            else
            {
                await ValidateFail(context, result);
            }
        }

        private async Task ValidateSuccess(ResourceOwnerPasswordValidationContext context, OAuthLoginResult result)
        {
            var client = _config.Clients.FirstOrDefault(w => w.Id == context.Request.Client.ClientId);
            result.User.ApiRole = client.Role;

            var claims = SetAdminClaims(result.User);
            if (client.IsWechat)
            {
                // 微信客户端
                var wxClient = await _wxClientRepository.GetAsync(w => w.ClientId == client.Id);
                if (wxClient == null)
                    throw new Exception("未配置微信客户端信息");

                var wxUser = await _wxUserManager.GetAsync(wxClient.AppId, result.User.Id);
                var wxClaims = SetWechatClaims(wxUser);
                claims.AddRange(wxClaims);
            }

            context.Result = new GrantValidationResult(context.UserName, OidcConstants.AuthenticationMethods.Password,
                claims, "local",
                new Dictionary<string, object>{
                {
                    OAuthConst.RESULT,
                    new BaseMessage().Success("登陆成功")
                }
            });
        }

        private async Task ValidateFail(ResourceOwnerPasswordValidationContext context, OAuthLoginResult result)
        {
            var msg = new BaseMessage().Fail();
            switch (result.ErrType)
            {
                case BaseErrType.NotAllow: msg.Message = "该账号已被冻结"; break;
                case BaseErrType.DataNotFound: msg.Message = "用户不存在"; break;
                case BaseErrType.PasswordInvalid: msg.Message = "密码输入错误，还可尝试{0}次".Fmt(result.LessPwdErrCount); break;
                case BaseErrType.Frozen: msg.Message = "密码输入错误次数过多，请{0}分钟后再尝试登陆".Fmt(Math.Round(result.LessBanTime)); break;
            }
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient, result.ErrType.ToString(), new Dictionary<string, object>
            {
                {
                    OAuthConst.RESULT,
                    msg
                }
            });
        }

        private List<Claim> SetAdminClaims(LoginUser user)
        {
            return user == null ? new List<Claim>() : new List<Claim> {
                    new Claim(UserClaimType.USERNAME, user.UserName),
                    new Claim(UserClaimType.USER_NICKNAME, user.Name),
                    new Claim(UserClaimType.USER_ID, user.Id.ToString()),
                    new Claim(UserClaimType.TENANT_ID, user.TenantId.ToString()),
                    new Claim(UserClaimType.PERSON_ID, user.PersonId.ToString()),
                    new Claim(UserClaimType.IS_DEFAULT, user.IsDefault.ToString()),
                    new Claim(UserClaimType.ROLE, user.ApiRole)
            };
        }

        private List<Claim> SetWechatClaims(SysWechatUser user)
        {
            return user == null ? new List<Claim>() : new List<Claim> {
                    new Claim(UserClaimType.WX_APPID, user.AppId),
                    new Claim(UserClaimType.WX_OPENID, user.OpenId),
                    new Claim(UserClaimType.WX_UNIONID, user.UnionId)
            };
        }
    }
}
