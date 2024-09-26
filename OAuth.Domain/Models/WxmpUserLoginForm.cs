using System.ComponentModel.DataAnnotations;

namespace OAuth.Domain.Models
{
    /// <summary>
    /// 微信小程序-登录
    /// </summary>
    public class WxmpUserLoginForm
    {
        /// <summary>
        /// 小程序代码
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        /// <summary>
        /// 小程序手机代码（与短信验证码二选一）
        /// </summary>
        [StringLength(50)]
        public string MobileCode { get; set; }

        /// <summary>
        /// 手机号码（与短信验证码使用）
        /// </summary>
        [StringLength(50)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 短信验证码（与小程序手机代码二选一）
        /// </summary>
        [StringLength(50)]
        public string SmsCode { get; set; }

        /// <summary>
        /// 客户端id
        /// </summary>
        [Required]
        public LoginClientForm Client { get; set; }

    }
}
