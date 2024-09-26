using Microsoft.EntityFrameworkCore;
using OAuth.Domain.AggregateRoots;
using OAuth.Domain.Repositorys;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Repository
{
    /// <summary>
    /// 风险账户
    /// </summary>
    public class SysUserRiskRecordRepository : Repository<SysUserRiskRecord>, ISysUserRiskRecordRepository
    {
        public SysUserRiskRecordRepository(DbContext context)
            : base(context)
        {

        }
    }
}
