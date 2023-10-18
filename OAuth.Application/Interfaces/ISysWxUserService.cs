using OAuth.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Application.Interfaces
{
    /// <summary>
    /// 微信用户
    /// </summary>
    public interface ISysWxUserService
    {
        /// <summary>
        /// 获取微信用户基础信息
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="clientId">客户端id</param>
        /// <returns></returns>
        Task<SysWxUserBaseDto> GetAsync(Guid userId, string clientId);
    }
}
