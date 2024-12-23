using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.EntityFrameworkCore;
using OAuth.Host.Models;
using Duende.IdentityServer.EntityFramework.DbContexts;

namespace OAuth.Host.Providers
{
    /// <summary>
    /// IdentityServer数据初始化
    /// </summary>
    public class IdentityServer4Provider
    {
        public readonly OAuthProvider _oAuthProvider;
        public readonly IServiceProvider _serviceProvider;
        public IdentityServer4Provider(
            OAuthProvider oAuthProvider,
            IServiceProvider serviceProvider)
        {
            _oAuthProvider = oAuthProvider;
            _serviceProvider = serviceProvider;
        }

        public void InitSeedData()
        {
            using (var scope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.Migrate();
                var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
            }
        }

        public void InitClientSeedData(ConfigurationDbContext context)
        {

        }
    }
}
