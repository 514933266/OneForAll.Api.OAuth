using System;
using System.Threading.Tasks;
using OAuth.Domain.AggregateRoots;
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
        Task<SysUser> GetWithTenantAsync(string username);

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="tenantId">机构id</param>
        /// <param name="username">用户名</param>
        /// <returns>系统用户</returns>
        Task<SysUser> GetWithTenantAsync(Guid tenantId, string username);

    }
}
