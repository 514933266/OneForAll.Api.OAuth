using SysLog.HttpService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.HttpService.Interfaces
{
    /// <summary>
    /// 授权登录服务
    /// </summary>
    public interface IIdentityServer4HttpService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="request">请求</param>
        /// <returns>登录结果</returns>
        Task<IdentityServer4Response> LoginAsync(IdentityServer4Request request);
    }
}
