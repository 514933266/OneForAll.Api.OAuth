using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.HttpService.Interfaces
{
    /// <summary>
    /// 腾讯云短信发送
    /// </summary>
    public interface ITxCloudSmsHttpService
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="signName">短信签名</param>
        /// <param name="content">短信内容</param>
        /// <param name="templateId">模板id</param>
        /// <param name="phoneNumber">手机号</param>
        /// <returns>登录结果</returns>
        Task<BaseMessage> SendAsync(string signName, string content, string templateId, string phoneNumber);
    }
}
