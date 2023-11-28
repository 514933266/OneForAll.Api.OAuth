using Microsoft.AspNetCore.Http;
using OAuth.HttpService.Interfaces;
using OAuth.HttpService.Models;
using OneForAll.Core.Extension;
using SysLog.HttpService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.HttpService
{
    /// <summary>
    /// 微信平台通用API
    /// </summary>
    public class WxHttpService : BaseHttpService, IWxHttpService
    {
        private readonly HttpServiceConfig _config;

        public WxHttpService(
            HttpServiceConfig config,
            IHttpContextAccessor httpContext,
            IHttpClientFactory httpClientFactory) : base(httpContext, httpClientFactory)
        {
            _config = config;
        }

        /// <summary>
        /// 获取接口调用凭据
        /// </summary>
        /// <param name="appId">AppId</param>
        /// <param name="appSecret">App密钥</param>
        /// <returns>登录结果</returns>
        public async Task<WxAccessTokenResponse> GetAccessTokenAsync(string appId, string appSecret)
        {
            var data = new WxAccessTokenResponse();
            var client = GetHttpClient(_config.WxmpAccessToken);
            if (client != null && client.BaseAddress != null)
            {
                var url = $"{client.BaseAddress}?appid={appId}&secret={appSecret}&grant_type=client_credential";
                var result = await client.GetAsync(new Uri(url));
                var str = await result.Content.ReadAsStringAsync();
                data = str.FromJson<WxAccessTokenResponse>();
            }
            return data;
        }
    }
}
