using OAuth.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// 小程序手机代码
        /// </summary>
        [Required]
        [StringLength(50)]
        public string MobileCode { get; set; }

        /// <summary>
        /// 客户端id
        /// </summary>
        [Required]
        public LoginClientForm Client { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public WxmpUserLoginBaseInfoForm UserInfo { get; set; }
    }

    /// <summary>
    /// 微信小程序-登录-用户信息
    /// </summary>
    public class WxmpUserLoginBaseInfoForm
    {
        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空
        /// </summary>
        [StringLength(1000)]
        public string AvatarUrl { get; set; }

        /// <summary>
        /// 用户的昵称
        /// </summary>
        [StringLength(20)]
        public string NickName { get; set; }
    }
}
