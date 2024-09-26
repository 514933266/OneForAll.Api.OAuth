using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Interfaces
{
    /// <summary>
    /// 验证码
    /// </summary>
    public interface ICaptchaManager
    {
        /// <summary>
        /// 设置验证码
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<string> SetStrCodeAsync(string username);

        /// <summary>
        /// 检查验证码
        /// </summary>
        /// <param name="username">账号</param>
        /// <param name="code">输入的验证码</param>
        /// <returns></returns>
        Task<bool> CheckStrCodeAsync(string username, string code);

        /// <summary>
        /// 检查是否存在验证码
        /// </summary>
        /// <param name="username">账号</param>
        /// <returns></returns>
        Task<bool> ExistsStrCodeAsync(string username);

        /// <summary>
        /// 获取Base64验证码
        /// </summary>
        /// <param name="userName">登录账号</param>
        /// <returns>结果</returns>
        Task<string> GetStrCodeBase64Async(string userName);
    }
}
