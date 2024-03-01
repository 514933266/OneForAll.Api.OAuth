using IdentityServer4.Models;
using OAuth.Host.Models;
using OAuth.Public.Models;
using OneForAll.Core.Extension;
using OneForAll.Core.OAuth;
using System.Collections.Generic;
using static IdentityServer4.IdentityServerConstants;

namespace OAuth.Host.Models
{
    // http://localhost:51512/.well-known/openid-configuration
    public class OAuthProvider
    {
        public static IEnumerable<IdentityResource> GetIdentityResources(OAuthProviderResource config)
        {
            return new List<IdentityResource>
            {
                 new IdentityResources.OpenId(),
                 new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources(OAuthProviderResource config)
        {
            var apis = new List<ApiResource>();
            config.ApiResources.ForEach(e =>
            {
                var api = new ApiResource()
                {
                    Name = e,
                    DisplayName = e,
                    Scopes = new List<string>() { e },
                    UserClaims = new List<string>() { UserClaimType.ROLE },
                };
                apis.Add(api);
            });
            return apis;
        }

        public static IEnumerable<Client> GetClients(OAuthProviderResource config)
        {
            var clients = new List<Client>();
            config.Clients.ForEach(e =>
            {
                var client = new Client
                {
                    ClientId = e.Id,
                    ClientName = e.Name,
                    ClientSecrets = { new Secret(e.Secret.Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword, // 密码登录模式
                    RefreshTokenUsage = TokenUsage.ReUse, // 刷新令牌时，refresh_token不变
                    RefreshTokenExpiration = TokenExpiration.Sliding, // 滑动过期策略
                    SlidingRefreshTokenLifetime = 1296000, // refresh_token 15天过期
                    AbsoluteRefreshTokenLifetime = 2592000, // refresh_token 30天绝对过期
                    AllowOfflineAccess = true, // 允许脱机访问
                    AccessTokenLifetime = 7200, // access_token 1小时过期
                    IdentityTokenLifetime = 7200, // 身份令牌 1小时过期
                    AuthorizationCodeLifetime = 7200, // 授权代码 1小时过期
                    UserSsoLifetime = 7200, // token无请求状态下 1小时续航
                    UpdateAccessTokenClaimsOnRefresh = true,// 使用refresh_token刷新时返回新的access_token
                    AllowedScopes = new List<string>()
                    {
                        e.Role,
                        StandardScopes.OfflineAccess,
                        StandardScopes.OpenId,
                        StandardScopes.Profile
                    }
                };
                clients.Add(client);
            });
            return clients;
        }

        public static IEnumerable<ApiScope> GetApiScopes(OAuthProviderResource config)
        {
            // 此处使用ApiResources，不对Scopes进行区分
            var scopes = new List<ApiScope>();
            config.ApiResources.ForEach(e =>
            {
                var scope = new ApiScope(e)
                {
                    Name = e,
                    DisplayName = e,
                    UserClaims = new List<string>() { "Policy" },
                };
                scopes.Add(scope);
            });
            return scopes;
        }
    }
}
