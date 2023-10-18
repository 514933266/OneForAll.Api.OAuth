using Microsoft.AspNetCore.Http;
using OAuth.HttpService;
using OAuth.HttpService.Models;
using OneForAll.Core.Extension;
using SysLog.HttpService.Interfaces;
using SysLog.HttpService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.HttpService
{
    /// <summary>
    /// 授权登录服务
    /// </summary>
    public class IdentityServer4HttpService : BaseHttpService, IIdentityServer4HttpService
    {
        private readonly HttpServiceConfig _config;

        public IdentityServer4HttpService(
            HttpServiceConfig config,
            IHttpContextAccessor httpContext,
            IHttpClientFactory httpClientFactory) : base(httpContext, httpClientFactory)
        {
            _config = config;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="request">请求</param>
        /// <returns>登录结果</returns>
        public async Task<IdentityServer4Response> LoginAsync(IdentityServer4Request request)
        {
            var client = GetHttpClient(_config.IdentityServer4Login);
            if (client != null && client.BaseAddress != null)
            {
                var url = $"{client.BaseAddress}";
                var body = new List<KeyValuePair<string, string>>();
                body.Add(new KeyValuePair<string, string>("grant_type", request.GrantType));
                body.Add(new KeyValuePair<string, string>("client_Id", request.ClientId));
                body.Add(new KeyValuePair<string, string>("client_secret", request.ClientSecret));
                body.Add(new KeyValuePair<string, string>("username", request.Username));
                body.Add(new KeyValuePair<string, string>("password", request.Password));

                var requestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(url),
                    Content = new FormUrlEncodedContent(body)
                };
                var result = await client.SendAsync(requestMessage);
                return await result.Content.ReadAsAsync<IdentityServer4Response>();
            }
            return new IdentityServer4Response();
        }
    }
}
