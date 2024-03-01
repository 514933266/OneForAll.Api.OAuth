using SysLog.HttpService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.HttpService.Interfaces
{
    /// <summary>
    /// 微信平台通用API
    /// </summary>
    public interface IWxHttpService
    {
        /// <summary>
        /// 获取接口调用凭据
        /// </summary>
        /// <param name="appId">AppId</param>
        /// <param name="appSecret">App密钥</param>
        /// <returns>登录结果</returns>
        Task<WxAccessTokenResponse> GetAccessTokenAsync(string appId, string appSecret);

        /// <summary>
        /// 获取稳定版AccessToken
        /// </summary>
        /// <param name="appId">AppId</param>
        /// <param name="appSecret">App密钥</param>
        /// <returns>登录结果</returns>
        Task<WxAccessTokenResponse> GetStableAccessTokenAsync(string appId, string appSecret);
    }
}
