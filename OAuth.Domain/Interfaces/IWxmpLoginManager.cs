using OAuth.Domain.Models;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Interfaces
{
    /// <summary>
    /// 微信小程序登录
    /// </summary>
    public interface IWxmpLoginManager
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>登录结果</returns>
        Task<BaseMessage> LoginAsync(WxmpUserLoginForm form);
    }
}
