using OAuth.Domain.AggregateRoots;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Interfaces
{
    /// <summary>
    /// 微信登录用户
    /// </summary>
    public interface ISysUserManager
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>结果</returns>
        Task<BaseErrType> AddAsync(SysUser entity);
    }
}
