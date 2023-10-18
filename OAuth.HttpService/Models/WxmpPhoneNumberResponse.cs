using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.HttpService.Models
{
    /// <summary>
    /// 微信小程序-用户手机号
    /// </summary>
    public class WxmpPhoneNumberResponse
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        [JsonProperty("errmsg")]
        public string ErrMsg { get; set; }

        /// <summary>
        /// 用户手机号详情
        /// </summary>
        [JsonProperty("phone_info")]
        public WxmpPhoneInfoResponse PhoneInfo { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        [JsonProperty("errcode")]
        public string ErrCode { get; set; }
    }

    /// <summary>
    /// 微信小程序-用户手机号详情
    /// </summary>
    public class WxmpPhoneInfoResponse
    {
        /// <summary>
        /// 用户绑定的手机号（国外手机号会有区号）
        /// </summary>
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 没有区号的手机号
        /// </summary>
        [JsonProperty("purePhoneNumber")]
        public string PurePhoneNumber { get; set; }

        /// <summary>
        /// 区号
        /// </summary>
        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }
    }
}
