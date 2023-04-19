using Microsoft.EntityFrameworkCore;
using OAuth.Domain.AggregateRoots;
using OneForAll.Core;
using OneForAll.Core.Extension;
using OneForAll.Core.ORM;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Repository
{
    /// <summary>
    /// 仓储：机构（租户）
    /// </summary>
    public class SysTenantRepository : Repository<SysTenant>, ISysTenantRepository
    {
        public SysTenantRepository(DbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// 查询机构（全表查询）
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>结果</returns>
        public async Task<SysTenant> GetByNameAsync(string name)
        {
            return await DbSet
                .IgnoreQueryFilters()
                .Where(w => w.Name.Equals(name))
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 查询机构（全表查询）
        /// </summary>
        /// <param name="code">机构信用代码</param>
        /// <returns>结果</returns>
        public async Task<SysTenant> GetByCodeAsync(string code)
        {
            return await DbSet
                .IgnoreQueryFilters()
                .Where(w => w.Code.Equals(code))
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 查询机构（全表查询）
        /// </summary>
        /// <param name="code">机构信用代码</param>
        /// <returns>结果</returns>
        public async Task<IEnumerable<SysTenant>> GetListByCodeAsync(IEnumerable<string> codes)
        {
            return await DbSet
                .IgnoreQueryFilters()
                .Where(w => codes.Contains(w.Code))
                .ToListAsync();
        }
    }
}
