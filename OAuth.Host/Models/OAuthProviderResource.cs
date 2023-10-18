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
        public List<string> ApiResources { get; set; } = new List<string>();

        /// <summary>
        /// 客户端
        /// </summary>
        public List<OAuthClient> Clients { get; set; } = new List<OAuthClient>();

        /// <summary>
        /// Api范围
        /// </summary>
        public List<string> ApiScopes { get; set; } = new List<string>();
    }
}
