using OneForAll.EFCore;
using OAuth.Domain.AggregateRoots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Repositorys
{
    /// <summary>
    /// 微信客户端
    /// </summary>
    public interface ISysWxClientSettingRepository : IEFCoreRepository<SysWxClientSetting>
    {
    }
}
