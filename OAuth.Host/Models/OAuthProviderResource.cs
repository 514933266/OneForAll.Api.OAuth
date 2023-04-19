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
        public ICollection<string> ApiResources { get; set; }

        /// <summary>
        /// 客户端
        /// </summary>
        public ICollection<OAuthClient> Clients { get; set; }
    }
}
