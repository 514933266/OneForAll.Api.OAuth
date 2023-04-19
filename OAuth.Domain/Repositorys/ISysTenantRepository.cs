using OAuth.Domain.AggregateRoots;
using OneForAll.Core;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Repository
{
    /// <summary>
    /// 仓储：机构（租户）
    /// </summary>
    public interface ISysTenantRepository : IEFCoreRepository<SysTenant>
    {

        /// <summary>
        /// 查询机构（全表查询）
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>结果</returns>
        Task<SysTenant> GetByNameAsync(string name);

        /// <summary>
        /// 查询机构（全表查询）
        /// </summary>
        /// <param name="code">机构信用代码</param>
        /// <returns>结果</returns>
        Task<SysTenant> GetByCodeAsync(string code);

        /// <summary>
        /// 查询机构（全表查询）
        /// </summary>
        /// <param name="code">机构信用代码</param>
        /// <returns>结果</returns>
        Task<IEnumerable<SysTenant>> GetListByCodeAsync(IEnumerable<string> codes);
    }
}
