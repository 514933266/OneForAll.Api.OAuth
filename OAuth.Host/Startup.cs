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
using OAuth.HttpService.Models;
using OAuth.Public.Models;
using System;
using OneForAll.Core.Utility;
using Quartz.Impl;
using Quartz.Spi;
using Quartz;
using OAuth.Host.Providers;
using OAuth.Host.QuartzJobs;
using OneForAll.Core.Upload;

namespace OAuth.Host
{
    public class Startup
    {
        private readonly string AUTH_SERVER = "IdentityServer";
        private readonly string CORS = "Cors";
        private const string QUARTZ = "Quartz";

        private readonly string BASE_HOST = "OAuth.Host";
        private readonly string BASE_DOMAIN = "OAuth.Domain";
        private readonly string BASE_APPLICATION = "OAuth.Application";
        private readonly string BASE_REPOSITORY = "OAuth.Repository";

        private readonly string HTTP_SERVICE_KEY = "HttpService";
        private readonly string HTTP_SERVICE = "OAuth.HttpService";

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

            #region Quartz
            var quartzConfig = new QuartzScheduleJobConfig();
            Configuration.GetSection(QUARTZ).Bind(quartzConfig);
            // 注册QuartzJobs目录下的定时任务
            if (quartzConfig != null)
            {
                services.AddSingleton(quartzConfig);
                services.AddSingleton<IJobFactory, ScheduleJobFactory>();
                services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
                services.AddHostedService<QuartzJobHostService>();
                var jobNamespace = BASE_HOST.Append(".QuartzJobs");
                quartzConfig.ScheduleJobs.ForEach(e =>
                {
                    var typeName = jobNamespace + "." + e.TypeName;
                    var jobType = Assembly.Load(BASE_HOST).GetType(typeName);
                    if (jobType != null)
                    {
                        e.JobType = jobType;
                        services.AddSingleton(e.JobType);
                    }
                });
            }
            #endregion

            #region Http

            var serviceConfig = new HttpServiceConfig();
            Configuration.GetSection(HTTP_SERVICE_KEY).Bind(serviceConfig);
            var props = OneForAll.Core.Utility.ReflectionHelper.GetPropertys(serviceConfig);
            props.ForEach(e =>
            {
                services.AddHttpClient(e.Name, c =>
                {
                    c.BaseAddress = new Uri(e.GetValue(serviceConfig).ToString());
                    c.DefaultRequestHeaders.Add("ClientId", ClientClaimType.Id);
                });
            });
            services.AddSingleton<HttpServiceConfig>();

            #endregion

            #region IdentityServer4
            
            var authServerConfig = new OAuthProviderResource();
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            var identityConnString = Configuration["ConnectionStrings:IdentityServer"];
            Configuration.GetSection(AUTH_SERVER).Bind(authServerConfig);
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(identityConnString, sql => sql.MigrationsAssembly(migrationsAssembly));
                    options.EnableTokenCleanup = true;
                })
                .AddInMemoryApiResources(OAuthProvider.GetApiResources(authServerConfig))
                .AddInMemoryClients(OAuthProvider.GetClients(authServerConfig))
                .AddInMemoryIdentityResources(OAuthProvider.GetIdentityResources(authServerConfig))
                .AddInMemoryApiScopes(OAuthProvider.GetApiScopes(authServerConfig))
                .AddResourceOwnerValidator<OAuthResourceOwnerPasswordValidator>()
                .AddProfileService<OAuthProfileService>();
            #endregion

            #region Redis
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration["Redis:ConnectionString"];
                options.InstanceName = Configuration["Redis:InstanceName"];
            });
            #endregion

            #region DI

            var authConfig = new AppInfo();
            Configuration.GetSection("App").Bind(authConfig);
            services.AddSingleton(authConfig);

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
            // Http
            builder.RegisterAssemblyTypes(Assembly.Load(HTTP_SERVICE))
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces();

            // 仓储
            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IEFCoreRepository<>));

            // 应用服务
            builder.RegisterAssemblyTypes(Assembly.Load(BASE_APPLICATION))
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            // 领域
            builder.RegisterAssemblyTypes(Assembly.Load(BASE_DOMAIN))
                .Where(t => t.Name.EndsWith("Manager"))
                .AsImplementedInterfaces();

            // 数据库上下文
            builder.RegisterType(typeof(OneForAll_BaseContext)).Named<DbContext>("OneForAll_BaseContext");
            builder.RegisterAssemblyTypes(Assembly.Load(BASE_REPOSITORY))
               .Where(t => t.Name.EndsWith("Repository"))
               .WithParameter(ResolvedParameter.ForNamed<DbContext>("OneForAll_BaseContext"))
               .AsImplementedInterfaces();

            // 登录配置
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
