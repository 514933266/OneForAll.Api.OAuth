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
    /// 微信客户端设置
    /// </summary>
    public class SysWxClientSettingRepository : Repository<SysWxClientSetting>, ISysWxClientSettingRepository
    {
        public SysWxClientSettingRepository(DbContext context)
            : base(context)
        {

        }
    }
}
