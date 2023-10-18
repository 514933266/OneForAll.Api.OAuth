using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.HttpService.Models
{
    /// <summary>
    /// IdentityServer4登录响应
    /// </summary>
    public class IdentityServer4Response
    {
        /// <summary>
        /// 错误码
        /// </summary>
        [JsonProperty("error")]
        public string Error { get; set; }

        /// <summary>
        /// 错误描述
        /// </summary>
        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }

        /// <summary>
        /// 令牌
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// 令牌时效
        /// </summary>
        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// 刷新token
        /// </summary>
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// 范围
        /// </summary>
        [JsonProperty("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// 额外提示结果
        /// </summary>
        [JsonProperty("result")]
        public object Result { get; set; }
    }
}
