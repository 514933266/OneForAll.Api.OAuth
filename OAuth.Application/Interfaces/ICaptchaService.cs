using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Application.Interfaces
{
    /// <summary>
    /// 验证码
    /// </summary>
    public interface ICaptchaService
    {
        /// <summary>
        /// 获取Base64验证码
        /// </summary>
        /// <param name="userName">登录账号</param>
        /// <returns>结果</returns>
        Task<string> GetImageBase64Async(string userName);
    }
}
