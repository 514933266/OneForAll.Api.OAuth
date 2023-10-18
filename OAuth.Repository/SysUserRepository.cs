using System;
using OAuth.Domain.AggregateRoots;
using Microsoft.EntityFrameworkCore;
using OneForAll.EFCore;
using System.Linq;
using System.Threading.Tasks;
using OAuth.Domain.Repositorys;
using OAuth.Domain.Aggregates;

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
        public async Task<SysLoginUserAggr> GetWithTenantAsync(string username)
        {
            var tenantDbSet = Context.Set<SysTenant>();

            var sql = (from user in DbSet.Where(w => w.UserName.Equals(username))
                       join tenant in tenantDbSet on user.SysTenantId equals tenant.Id into leftJoinTenant
                       from lfTenant in leftJoinTenant.DefaultIfEmpty()
                       select new SysLoginUserAggr()
                       {
                           Id = user.Id,
                           Name = user.Name,
                           UserName = user.UserName,
                           IconUrl = user.IconUrl,
                           IsDefault = user.IsDefault,
                           LastLoginIp = user.LastLoginIp,
                           LastLoginTime = user.LastLoginTime,
                           Password = user.Password,
                           PwdErrCount = user.PwdErrCount,
                           Signature = user.Signature,
                           Status = user.Status,
                           SysTenantId = (lfTenant == null ? Guid.Empty : lfTenant.Id),
                           UpdateTime = user.UpdateTime,
                           SysTenant = lfTenant
                       });

            return await sql.FirstOrDefaultAsync();
        }
    }
}
