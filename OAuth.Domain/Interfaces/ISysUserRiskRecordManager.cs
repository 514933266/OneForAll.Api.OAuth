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
    /// 用户风险记录
    /// </summary>
    public interface ISysUserRiskRecordManager
    {
        /// <summary>
        /// 获取一条风险记录
        /// </summary>
        /// <param name="username">账号</param>
        /// <returns>结果</returns>
        Task<SysUserRiskRecord> GetAsync(string username);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>结果</returns>
        Task<BaseErrType> AddAsync(SysUserRiskRecord entity);
    }
}
