using OAuth.Domain.AggregateRoots;
using OAuth.Domain.Aggregates;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Repositorys
{
    /// <summary>
    /// 网站设置
    /// </summary>
    public interface ISysWebsiteSettingRepository : IEFCoreRepository<SysWebsiteSetting>
    {
        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="host">域名</param>
        /// <returns>实体</returns>
        Task<SysWebsiteSettingAggr> GetByHostWithContactAsync(string host);
    }
}
