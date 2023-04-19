using System.Reflection;
using Autofac;
using Autofac.Core;
using AutoMapper;
using OAuth.Domain.ValueObjects;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OAuth.Host.Models;
using OneForAll.Core.Extension;
using OneForAll.EFCore;
using System.Collections.Generic;

namespace OAuth.Host
{
    public class Startup
    {
        private readonly string AUTH_SERVER = "AuthServer";
        private readonly string CORS = "Cors";
        private readonly string BASE_HOST = "OAuth.Host";
        private readonly string BASE_DOMAIN = "OAuth.Domain";
        private readonly string BASE_REPOSITORY = "OAuth.Repository";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            #region Cors

            var corsConfig = new CorsConfig();
            Configuration.GetSection(CORS).Bind(corsConfig);
            services.AddCors(option => option.AddPolicy(CORS, policy => policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
            ));

            #endregion

            #region AutoMapper
            services.AddAutoMapper(Assembly.Load(BASE_HOST));
            #endregion

            #region EFCore

            services.AddDbContext<OneForAll_BaseContext>(options =>
                     options.UseSqlServer(Configuration["ConnectionStrings:Default"]));
            #endregion

            #region IdentityServer4

            // 使用默认证书
            var authServerConfig = new OAuthProviderResource();
            Configuration.GetSection(AUTH_SERVER).Bind(authServerConfig);
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(OAuthProvider.GetApiResource(authServerConfig))
                .AddInMemoryClients(OAuthProvider.GetClients(authServerConfig))
                .AddInMemoryIdentityResources(OAuthProvider.GetIdentityResource())
                .AddResourceOwnerValidator<OAuthResourceOwnerPasswordValidator>()
                .AddProfileService<OAuthProfileService>();
            #endregion

            #region Mvc

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllers(options =>
            {
                options.EnableEndpointRouting = false;
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            #endregion

        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IEFCoreRepository<>));

            builder.RegisterAssemblyTypes(Assembly.Load(BASE_DOMAIN))
                .Where(t => t.Name.EndsWith("Manager"))
                .AsImplementedInterfaces();

            builder.RegisterType(typeof(OneForAll_BaseContext)).Named<DbContext>("OneForAll_BaseContext");
            builder.RegisterAssemblyTypes(Assembly.Load(BASE_REPOSITORY))
               .Where(t => t.Name.EndsWith("Repository"))
               .WithParameter(ResolvedParameter.ForNamed<DbContext>("OneForAll_BaseContext"))
               .AsImplementedInterfaces();

            builder.Register(s =>
                new OAuthLoginSetting()
                {
                    BanTime = Configuration["LoginSetting:BanTime"].TryInt(),
                    MaxPwdErrCount = Configuration["LoginSetting:MaxPwdErrCount"].TryInt()
                }).SingleInstance();

            var authConfig = new OAuthProviderResource();
            Configuration.GetSection(AUTH_SERVER).Bind(authConfig);
            builder.Register(s => authConfig).SingleInstance();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(CORS);

            app.UseRouting();

            app.UseIdentityServer();
           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
