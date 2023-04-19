using IdentityServer4.Models;
using OAuth.Host.Models;
using OneForAll.Core.Extension;
using System.Collections.Generic;
using static IdentityServer4.IdentityServerConstants;

namespace OAuth.Host.Models
{
    // http://localhost:51512/.well-known/openid-configuration
    public class OAuthProvider
    {
        public static IEnumerable<IdentityResource> GetIdentityResource()
        {
            return new List<IdentityResource>
            {
                 new IdentityResources.OpenId(),
                 new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApiResource(OAuthProviderResource config)
        {
            var apis = new List<ApiResource>();
            config.ApiResources.ForEach(e =>
            {
                var api = new ApiResource(e, new List<string> { OAuthUserClaimType.ROLE });
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
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    AllowOfflineAccess = true,
                    AllowedScopes = new List<string>
                    {
                            StandardScopes.OfflineAccess,
                            StandardScopes.OpenId,
                            StandardScopes.Profile
                    }
                };
                e.Scopes.ForEach(scope =>
                {
                    client.AllowedScopes.Add(scope);
                });
                clients.Add(client);
            });
            return clients;
        }
    }
}
