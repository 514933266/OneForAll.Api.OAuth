using OAuth.Domain.Models;
using OAuth.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Application.Interfaces
{
    /// <summary>
    /// 登录
    /// </summary>
    public interface IOAuthLoginService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginUser">登录信息</param>
        /// <returns>结果</returns>
        Task<OAuthLoginResultVo> LoginAsync(OAuthLoginVo loginUser);
    }
}
