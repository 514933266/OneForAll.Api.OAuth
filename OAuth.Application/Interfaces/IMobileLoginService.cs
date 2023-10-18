using OAuth.Domain.Models;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Application.Interfaces
{
    /// <summary>
    /// 手机号登录
    /// </summary>
    public interface IMobileLoginService
    {
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="phoneNumber">手机号码</param>
        /// <returns>登录结果</returns>
        Task<BaseMessage> GetCodeAsync(string phoneNumber);

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        Task<BaseMessage> LoginAsync(MobileLoginForm form);
    }
}
