﻿using System.Reflection;
using Autofac;
using Autofac.Core;
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
using Quartz.Impl;
using Quartz.Spi;
using Quartz;
using OAuth.Host.Providers;
using OAuth.Host.Filters;
using System.Linq;

namespace OAuth.Host
{
    public class Startup
    {
        private const string CORS = "Cors";
        private const string AUTH = "Auth";
        private const string QUARTZ = "Quartz";

        private const string BASE_HOST = "OAuth.Host";
        private const string BASE_DOMAIN = "OAuth.Domain";
        private const string BASE_APPLICATION = "OAuth.Application";
        private const string BASE_REPOSITORY = "OAuth.Repository";

        private const string HTTP_SERVICE_KEY = "HttpService";
        private const string HTTP_SERVICE = "OAuth.HttpService";

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

            #region Http

            var serviceConfig = new HttpServiceConfig();
            Configuration.GetSection(HTTP_SERVICE_KEY).Bind(serviceConfig);
            var props = OneForAll.Core.Utility.ReflectionHelper.GetPropertys(serviceConfig);
            props.ForEach(e =>
            {
                var url = e.GetValue(serviceConfig).ToString();
                if (url.IsNullOrEmpty()) return;
                services.AddHttpClient(e.Name, c =>
                {
                    c.BaseAddress = new Uri(url);
                    c.DefaultRequestHeaders.Add("ClientId", ClientClaimType.Id);
                });
            });
            services.AddSingleton<HttpServiceConfig>();

            #endregion

            #region DI

            var authConfig = new AuthConfig();
            Configuration.GetSection(AUTH).Bind(authConfig);
            services.AddSingleton(authConfig);

            #endregion

            #region IdentityServer4

            var connStr = Configuration["ConnectionStrings:Default"];
            var authServerConfig = new OAuthProviderResource();
            var dbOptions = new DbContextOptionsBuilder<SysDbContext>().UseSqlServer(connStr).Options;
            services.AddSingleton(authServerConfig);
            using (var context = new SysDbContext(dbOptions))
            {
                // 如果表中有配置客户端，则覆盖json文件配置
                var clients = context.SysClient.ToList();
                if (clients.Any())
                    authServerConfig.Init(clients);

                services.AddIdentityServer(option =>
                {
                    option.IssuerUri = authConfig.Issuer;
                })
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(OAuthProvider.GetApiResources(authServerConfig))
                .AddInMemoryClients(OAuthProvider.GetClients(authServerConfig))
                .AddInMemoryIdentityResources(OAuthProvider.GetIdentityResources(authServerConfig))
                .AddInMemoryApiScopes(OAuthProvider.GetApiScopes(authServerConfig))
                .AddResourceOwnerValidator<OAuthResourceOwnerPasswordValidator>()
                .AddProfileService<OAuthProfileService>();
            }

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

            #region AutoMapper
            services.AddAutoMapper(Assembly.Load(BASE_HOST));
            #endregion

            #region Redis
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration["Redis:ConnectionString"];
                options.InstanceName = Configuration["Redis:InstanceName"];
                if (!Configuration["Redis:Password"].IsNullOrEmpty())
                    options.ConfigurationOptions.Password = Configuration["Redis:Password"];
            });
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
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Http请求服务
            builder.RegisterAssemblyTypes(Assembly.Load(HTTP_SERVICE))
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces();

            // 仓储
            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IEFCoreRepository<>));

            // 应用服务层
            builder.RegisterAssemblyTypes(Assembly.Load(BASE_APPLICATION))
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            // 领域层
            builder.RegisterAssemblyTypes(Assembly.Load(BASE_DOMAIN))
                .Where(t => t.Name.EndsWith("Manager"))
                .AsImplementedInterfaces();

            // 仓储层
            builder.Register(p =>
            {
                var optionBuilder = new DbContextOptionsBuilder<SysDbContext>();
                optionBuilder.UseSqlServer(Configuration["ConnectionStrings:Default"]);
                return optionBuilder.Options;
            }).AsSelf();
            builder.RegisterType(typeof(SysDbContext)).Named<DbContext>("SysDbContext");
            builder.RegisterAssemblyTypes(Assembly.Load(BASE_REPOSITORY))
               .Where(t => t.Name.EndsWith("Repository"))
               .WithParameter(ResolvedParameter.ForNamed<DbContext>("SysDbContext"))
               .AsImplementedInterfaces();

            // 登录配置
            builder.Register(s =>
                new OAuthLoginSettingVo()
                {
                    BanTime = Configuration["LoginSetting:BanTime"].TryInt(),
                    MaxPwdErrCount = Configuration["LoginSetting:MaxPwdErrCount"].TryInt(),
                    IsCaptchaEnabled = Configuration["LoginSetting:IsCaptchaEnabled"].TryBoolean(),
                    IgnoreCaptchaKey = Configuration["LoginSetting:IgnoreCaptchaKey"],
                }).SingleInstance();
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
            app.UseMiddleware<ApiLogMiddleware>();
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
