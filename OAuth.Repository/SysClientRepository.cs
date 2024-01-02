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
    /// 系统客户端
    /// </summary>
    public class SysClientRepository : Repository<SysClient>, ISysClientRepository
    {
        public SysClientRepository(DbContext context)
            : base(context)
        {

        }
    }
}
