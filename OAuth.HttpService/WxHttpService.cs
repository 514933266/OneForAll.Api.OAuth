using Microsoft.AspNetCore.Http;
using OAuth.HttpService.Interfaces;
using OAuth.HttpService.Models;
using OneForAll.Core.Extension;
using Org.BouncyCastle.Asn1.Ocsp;
using SysLog.HttpService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
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
        /// 获取AccessToken
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

        /// <summary>
        /// 获取稳定版AccessToken
        /// </summary>
        /// <param name="appId">AppId</param>
        /// <param name="appSecret">App密钥</param>
        /// <returns>登录结果</returns>
        public async Task<WxAccessTokenResponse> GetStableAccessTokenAsync(string appId, string appSecret)
        {
            var client = GetHttpClient(_config.WxStableAccessToken);
            if (client != null && client.BaseAddress != null)
            {
                // 微信接口获取手机号会检测Content-Length请求头，而.net7使用流请求才会携带，否则响应412
                var url = $"{client.BaseAddress}";
                var requestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(url),
                    Content = new ByteArrayContent(Encoding.UTF8.GetBytes(new
                    {
                        grant_type = "client_credential",
                        appid = appId,
                        secret = appSecret
                    }.ToJson()))
                };
                var result = await client.SendAsync(requestMessage);
                var data = await result.Content.ReadAsAsync<WxAccessTokenResponse>();
                if (data == null)
                    throw new Exception(result.Content.ReadAsStringAsync().Result);
                return data;
            }
            return null;
        }
    }
}
