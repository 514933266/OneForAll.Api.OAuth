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
    /// 微信客户端设置
    /// </summary>
    public class SysWxClientRepository : Repository<SysWxClient>, ISysWxClientRepository
    {
        public SysWxClientRepository(DbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// 查询指定客户端对应的微信客户端信息
        /// </summary>
        /// <param name="clientId">系统客户端</param>
        /// <returns>系统用户</returns>
        public async Task<SysWxClientAggr> GetByClientIdAsync(string clientId)
        {
            var clientDbSet = Context.Set<SysClient>().Where(w => w.ClientId == clientId);
            var contactDbSet = Context.Set<SysWxClientContact>();
            var sql = (from client in clientDbSet
                       join contact in contactDbSet on client.Id equals contact.SysClientId
                       join wxClient in DbSet on contact.SysWxClientId equals wxClient.Id
                       select new SysWxClientAggr()
                       {
                           Id = wxClient.Id,
                           AppId = wxClient.AppId,
                           AppSecret = wxClient.AppSecret,
                           AccessToken = wxClient.AccessToken,
                           AccessTokenExpiresIn = wxClient.AccessTokenExpiresIn,
                           AccessTokenCreateTime = wxClient.AccessTokenCreateTime,
                           SysClient = client
                       });

            return await sql.FirstOrDefaultAsync();
        }
    }
}
