using OAuth.Domain.AggregateRoots;
using OAuth.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Interfaces
{
    /// <summary>
    /// 手机号登录
    /// </summary>
    public interface IMobileLoginManager
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        Task<SysUser> LoginAsync(MobileLoginForm form);
    }
}
