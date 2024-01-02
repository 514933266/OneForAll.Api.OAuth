using OAuth.Domain.AggregateRoots;
using OAuth.Domain.Enums;
using OneForAll.Core.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth.Host.Models
{
    /// <summary>
    /// OAuth授权配置
    /// </summary>
    public class OAuthProviderResource
    {
        /// <summary>
        /// Api
        /// </summary>
        public HashSet<string> ApiResources { get; set; } = new HashSet<string>();

        /// <summary>
        /// 客户端
        /// </summary>
        public HashSet<OAuthClient> Clients { get; set; } = new HashSet<OAuthClient>();

        /// <summary>
        /// Api范围
        /// </summary>
        public HashSet<string> ApiScopes { get; set; } = new HashSet<string>();

        /// <summary>
        /// 根据系统客户端初始化
        /// </summary>
        /// <param name="clients">系统客户端</param>
        public void Init(IEnumerable<SysClient> clients)
        {
            var wxTypeArr = new[] { SysClientTypeEnum.Wxmp, SysClientTypeEnum.Wxgzh };
            clients.ForEach(e =>
            {
                Clients.Add(new OAuthClient()
                {
                    Id = e.ClientId,
                    Name = e.ClientName,
                    Secret = e.ClientSecret,
                    Role = e.Role,
                    IsWechat = wxTypeArr.Contains(e.Type),
                });
                ApiResources.Add(e.Role);
                ApiScopes.Add(e.Role);
            });
        }
    }
}
