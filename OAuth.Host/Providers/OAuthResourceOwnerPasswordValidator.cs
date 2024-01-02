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
using OAuth.Domain.Enums;
using TencentCloud.Mna.V20210119.Models;

namespace OAuth.Host.Providers
{
    public class OAuthResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IOAuthLoginManager _loginManager;
        private readonly ISysWxUserManager _wxUserManager;
        private readonly IHttpContextAccessor _accessor;
        private readonly ISysClientRepository _clientRepository;
        private readonly ISysWxClientRepository _wxClientRepository;
        public OAuthResourceOwnerPasswordValidator(
            ISysWxUserManager wxUserManager,
            IOAuthLoginManager loginManager,
            IHttpContextAccessor accessor,
            ISysClientRepository clientRepository,
            ISysWxClientRepository wxClientRepository)
        {
            _accessor = accessor;
            _wxUserManager = wxUserManager;
            _loginManager = loginManager;
            _clientRepository = clientRepository;
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
            var msg = new BaseMessage();
            var client = await _clientRepository.GetAsync(w => w.ClientId == context.Request.Client.ClientId);
            if (client == null)
                throw new Exception("未配置系统客户端信息");

            result.User.ApiRole = client.Role;

            var claims = SetAdminClaims(result.User);

            #region 微信客户端信息

            if (client.Type == SysClientTypeEnum.Wxmp || client.Type == SysClientTypeEnum.Wxgzh)
            {
                // 微信客户端
                var wxClient = await _wxClientRepository.GetByClientIdAsync(client.ClientId);
                if (wxClient == null)
                    throw new Exception("未配置微信客户端信息");

                var wxUser = await _wxUserManager.GetAsync(wxClient.AppId, result.User.Id);
                var wxClaims = SetWechatClaims(wxUser);
                claims.AddRange(wxClaims);
            }
            #endregion

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
                case BaseErrType.NotAllow:
                    msg.Message = "账号已被永久封禁";
                    break;
                case BaseErrType.DataNotFound: msg.Message = "账号不存在"; break;
                case BaseErrType.DataError: msg.Message = "账号异常，请联系管理员"; break;
                case BaseErrType.PermissionNotEnough: msg.Message = "账号权限不足"; break;
                case BaseErrType.PasswordInvalid: msg.Message = "密码输入错误，还可尝试{0}次".Fmt(result.LessPwdErrCount); break;
                case BaseErrType.Frozen: msg.Message = "账号已被冻结，请{0}分钟后再尝试登陆".Fmt(Math.Round(result.LessBanTime)); break;
            }
            context.Result = GetResult(TokenRequestErrors.InvalidClient, msg);
        }

        // 获取响应值
        private GrantValidationResult GetResult(TokenRequestErrors error, BaseMessage msg)
        {
            return new GrantValidationResult(error, msg.ErrType.ToString(), new Dictionary<string, object>
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

        private List<Claim> SetWechatClaims(SysWxUser user)
        {
            return user == null ? new List<Claim>() : new List<Claim> {
                    new Claim(UserClaimType.WX_APPID, user.AppId),
                    new Claim(UserClaimType.WX_OPENID, user.OpenId),
                    new Claim(UserClaimType.WX_UNIONID, user.UnionId)
            };
        }
    }
}
