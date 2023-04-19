using System;
using OAuth.Domain.AggregateRoots;
using Microsoft.EntityFrameworkCore;
using OneForAll.EFCore;
using System.Linq;
using System.Threading.Tasks;
using OAuth.Domain.Repositorys;

namespace OAuth.Repository
{
    /// <summary>
    /// 仓储：用户
    /// </summary>
    public class SysUserRepository : Repository<SysUser>, ISysUserRepository
    {
        public SysUserRepository(DbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>系统用户</returns>
        public async Task<SysUser> GetWithTenantAsync(string username)
        {
            return await DbSet
                .Where(w => w.UserName.Equals(username))
                .Include(e => e.SysTenant)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="tenantId">机构id</param>
        /// <param name="username">用户名</param>
        /// <returns>系统用户</returns>
        public async Task<SysUser> GetWithTenantAsync(Guid tenantId, string username)
        {
            return await DbSet
                .Where(w => w.UserName.Equals(username) && w.SysTenantId == tenantId)
                .Include(e => e.SysTenant)
                .FirstOrDefaultAsync();
        }
    }
}
