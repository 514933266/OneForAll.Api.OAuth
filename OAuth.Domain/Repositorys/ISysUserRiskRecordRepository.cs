using OAuth.Domain.AggregateRoots;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Repositorys
{
    /// <summary>
    /// 风险账户
    /// </summary>
    public interface ISysUserRiskRecordRepository : IEFCoreRepository<SysUserRiskRecord>
    {
    }
}
