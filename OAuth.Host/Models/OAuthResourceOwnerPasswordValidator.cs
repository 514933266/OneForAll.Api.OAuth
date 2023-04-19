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

namespace OAuth.Host.Models
{
    public class OAuthResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly OAuthProviderResource _config;
        private readonly IOAuthLoginManager _loginManager;
        private readonly IHttpContextAccessor _accessor;
        public OAuthResourceOwnerPasswordValidator(
            OAuthProviderResource config,
            IOAuthLoginManager loginManager,
            IHttpContextAccessor accessor)
        {
            _config = config;
            _accessor = accessor;
            _loginManager = loginManager;
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
                ValidateSuccess(context, result);
            }
            else
            {
                ValidateFail(context, result);
            }
        }

        private void ValidateSuccess(ResourceOwnerPasswordValidationContext context, OAuthLoginResult result)
        {
            var claims = SetAdminClaims(context, result.User);
            context.Result = new GrantValidationResult(context.UserName, OidcConstants.AuthenticationMethods.Password,
                claims, "local",
                new Dictionary<string, object>
                {
                {
                    OAuthConst.RESULT,
                    new BaseMessage().Success("登陆成功")
                }
            });
        }

        private void ValidateFail(ResourceOwnerPasswordValidationContext context, OAuthLoginResult result)
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

        private IEnumerable<Claim> SetAdminClaims(ResourceOwnerPasswordValidationContext context, OAuthLoginUser user)
        {
            // 当为默认机构的默认用户登录时，角色设为开发人员
            var client = _config.Clients.FirstOrDefault(w => w.Id == context.Request.Client.ClientId);
            return new List<Claim> {
                    new Claim(OAuthUserClaimType.USERNAME, user.UserName),
                    new Claim(OAuthUserClaimType.USER_NICKNAME, user.Name),
                    new Claim(OAuthUserClaimType.USER_ID, user.Id.ToString()),
                    new Claim(OAuthUserClaimType.TENANT_ID, user.TenantId.ToString()),
                    new Claim(OAuthUserClaimType.PERSON_ID, user.PersonId.ToString()),
                    new Claim(OAuthUserClaimType.IS_DEFAULT, user.IsDefault.ToString()),
                    new Claim(OAuthUserClaimType.ROLE, (user.IsDefault && user.IsDefaultTenant)? OAuthUserRoleType.RULER: client.Role)
            };
        }
    }
}
