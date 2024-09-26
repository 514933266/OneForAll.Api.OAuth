using System;
using System.Threading.Tasks;
using OAuth.Domain.AggregateRoots;
using OAuth.Domain.Aggregates;
using OneForAll.EFCore;

namespace OAuth.Domain.Repositorys
{
    /// <summary>
    /// 仓储：用户
    /// </summary>
    public interface ISysUserRepository : IEFCoreRepository<SysUser>
    {
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>系统用户</returns>
        Task<SysUser> GetAsync(string username);

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>系统用户</returns>
        Task<SysLoginUserAggr> GetWithTenantAsync(string username);

    }
}
