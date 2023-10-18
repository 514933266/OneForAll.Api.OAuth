using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OAuth.HttpService.Models;
using OAuth.HttpService;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SysLog.HttpService.Interfaces;
using OneForAll.Core.Extension;
using System.Net.Http.Formatting;
using SysLog.HttpService.Models;

namespace SysLog.HttpService
{
    /// <summary>
    /// 微信小程序
    /// </summary>
    public class WxmpHttpService : BaseHttpService, IWxmpHttpService
    {
        private readonly HttpServiceConfig _config;

        public WxmpHttpService(
            HttpServiceConfig config,
            IHttpContextAccessor httpContext,
            IHttpClientFactory httpClientFactory) : base(httpContext, httpClientFactory)
        {
            _config = config;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="appId">AppId</param>
        /// <param name="appSecret">App密钥</param>
        /// <param name="code">微信客户端拿到的code</param>
        /// <returns>登录结果</returns>
        public async Task<BaseMessage> LoginAsync(string appId, string appSecret, string code)
        {
            var msg = new BaseMessage() { Status = false, Message = "微信服务地址未配置！", ErrType = BaseErrType.Fail };
            var client = GetHttpClient(_config.WxmpLogin2Session);
            if (client != null && client.BaseAddress != null)
            {
                if (appId.IsNullOrEmpty() || appSecret.IsNullOrEmpty())
                {
                    msg.Message = "微信App配置异常！";
                    return msg;
                }
                var url = $"{client.BaseAddress}?appid={appId}&secret={appSecret}&js_code={code}&grant_type=authorization_code";
                var result = await client.GetAsync(new Uri(url));
                var response = await result.Content.ReadAsStringAsync();
                var data = response.FromJson<WxmpLogin2SessionResponse>();
                if (!data.SessionKey.IsNullOrEmpty())
                {
                    msg.Status = true;
                    msg.Data = data;
                    msg.ErrType = BaseErrType.Success;
                    msg.Message = "微信登录成功";
                }
                else
                {
                    msg.Message = data.ErrMsg;
                }
            }
            return msg;
        }

        /// <summary>
        /// 获取手机号
        /// </summary>
        /// <param name="request">手机号授权请求内容</param>
        /// <param name="access_token">接口调用凭据</param>
        /// <returns>登录结果</returns>
        public async Task<string> GetPhoneNumberAsync(WxmpPhoneNumberRequest request, string access_token)
        {
            var phoneNumber = "";
            var client = GetHttpClient(_config.WxmpPhoneNumber);
            if (client != null && client.BaseAddress != null)
            {
                // 微信接口获取手机号会检测Content-Length请求头，而.net7使用流请求才会携带，否则响应412
                var url = $"{client.BaseAddress}?access_token={access_token}";
                var requestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(url),
                    Content = new ByteArrayContent(Encoding.UTF8.GetBytes(request.ToJson()))
                };
                var result = await client.SendAsync(requestMessage);
                var data = await result.Content.ReadAsAsync<WxmpPhoneNumberResponse>();
                if (data.ErrCode == "0")
                {
                    phoneNumber = data.PhoneInfo.PurePhoneNumber;
                }
                else
                {
                    throw new Exception($"获取手机号失败，错误码{data.ErrMsg}");
                }
            }
            return phoneNumber;
        }
    }
}
