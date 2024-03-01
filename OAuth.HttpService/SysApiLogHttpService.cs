using Microsoft.AspNetCore.Http;
using OAuth.HttpService.Interfaces;
using OAuth.HttpService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.HttpService
{
    /// <summary>
    /// Api日志
    /// </summary>
    public class SysApiLogHttpService : BaseHttpService, ISysApiLogHttpService
    {
        private readonly HttpServiceConfig _config;

        public SysApiLogHttpService(
            HttpServiceConfig config,
            IHttpContextAccessor httpContext,
            IHttpClientFactory httpClientFactory) : base(httpContext, httpClientFactory)
        {
            _config = config;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns></returns>
        public async Task AddAsync(SysApiLogRequest form)
        {
            form.CreatorId = LoginUser.Id;
            form.CreatorName = LoginUser.Name;
            form.TenantId = LoginUser.SysTenantId;
            form.CreateTime = DateTime.Now;

            var client = GetHttpClient(_config.SysApiLog);
            if (client != null && client.BaseAddress != null)
            {
                await client.PostAsync(client.BaseAddress, form, new JsonMediaTypeFormatter());
            }
        }
    }
}

