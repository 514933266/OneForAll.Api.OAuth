using Microsoft.EntityFrameworkCore;
using OAuth.Domain.AggregateRoots;
using OAuth.Domain.Aggregates;
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
    /// 网站设置
    /// </summary>
    public class SysWebsiteSettingRepository : Repository<SysWebsiteSetting>, ISysWebsiteSettingRepository
    {
        public SysWebsiteSettingRepository(DbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="host">域名</param>
        /// <returns>实体</returns>
        public async Task<SysWebsiteSettingAggr> GetByHostWithContactAsync(string host)
        {
            var apiDbSet = Context.Set<SysWebsiteApiSetting>();

            var query = (from web in DbSet
                         join api in apiDbSet on web.Id equals api.SysWebsiteSettingId
                         where web.Host == host
                         group new
                         {
                             web.Id,
                             web.Host,
                             web.Name,
                             web.OAuthClientId,
                             web.OAuthClientSecret,
                             web.OAuthClientName,
                             web.OAuthUrl,
                             web.LoginBackgroundUrl,
                             Api = api
                         } by web.Id into gData
                         select new SysWebsiteSettingAggr()
                         {
                             Id = gData.FirstOrDefault().Id,
                             Host = gData.FirstOrDefault().Host,
                             Name = gData.FirstOrDefault().Name,
                             OAuthClientId = gData.FirstOrDefault().OAuthClientId,
                             OAuthClientSecret = gData.FirstOrDefault().OAuthClientSecret,
                             OAuthClientName = gData.FirstOrDefault().OAuthClientName,
                             OAuthUrl = gData.FirstOrDefault().OAuthUrl,
                             LoginBackgroundUrl = gData.FirstOrDefault().LoginBackgroundUrl,
                             SysWebsiteApiSettings = gData.Select(s => s.Api).ToList()
                         });
            return await query.FirstOrDefaultAsync();
        }
    }
}
