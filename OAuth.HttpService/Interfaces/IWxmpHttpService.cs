using OAuth.HttpService.Models;
using OneForAll.Core;
using SysLog.HttpService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.HttpService.Interfaces
{
    /// <summary>
    /// 微信小程序
    /// </summary>
    public interface IWxmpHttpService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="appId">AppId</param>
        /// <param name="appSecret">App密钥</param>
        /// <param name="code">微信客户端拿到的code</param>
        /// <returns>登录结果</returns>
        Task<BaseMessage> LoginAsync(string appId, string appSecret, string code);

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="request">手机号授权请求内容</param>
        /// <param name="access_token">接口调用凭据</param>
        /// <returns>登录结果</returns>
        Task<string> GetPhoneNumberAsync(WxmpPhoneNumberRequest request, string access_token);
    }
}
