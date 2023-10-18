using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.HttpService.Models
{
    /// <summary>
    /// 微信小程序登录
    /// </summary>
    public class WxmpLogin2SessionResponse
    {
        /// <summary>
        /// 会话密钥
        /// </summary>
        [JsonProperty("session_key")]
        public string SessionKey { get; set; }

        /// <summary>
        /// 用户在开放平台的唯一标识符，若当前小程序已绑定到微信开放平台账号下会返回
        /// </summary>
        [JsonProperty("unionid")]
        public string UnionId { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        [JsonProperty("errmsg")]
        public string ErrMsg { get; set; }

        /// <summary>
        /// 用户唯一标识
        /// </summary>
        [JsonProperty("openid")]
        public string OpenId { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        [JsonProperty("errcode")]
        public string ErrCode { get; set; }
    }
}
