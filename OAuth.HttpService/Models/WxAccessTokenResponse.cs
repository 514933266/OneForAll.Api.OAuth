using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.HttpService.Models
{
    /// <summary>
    /// 微信小程序-接口调用凭据
    /// </summary>
    public class WxAccessTokenResponse
    {
        /// <summary>
        /// 获取到的凭证
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// 凭证有效时间，单位：秒。目前是7200秒之内的值
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
