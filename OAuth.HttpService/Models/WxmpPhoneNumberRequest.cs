using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.HttpService.Models
{
    /// <summary>
    /// 微信小程序-获取用户手机号
    /// </summary>
    public class WxmpPhoneNumberRequest
    {
        /// <summary>
        /// 手机号获取凭证
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
