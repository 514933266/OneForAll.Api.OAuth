using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.HttpService.Models
{
    /// <summary>
    /// IdentityServer4登录请求
    /// </summary>
    public class IdentityServer4Request
    {
        /// <summary>
        /// 类型
        /// </summary>
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }

        /// <summary>
        /// 客户端Id
        /// </summary>
        [JsonProperty("client_Id")]
        public string ClientId { get; set; }

        /// <summary>
        /// 客户端密码
        /// </summary>
        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        /// <summary>
        /// 客户端密码
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// 客户端密码
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
