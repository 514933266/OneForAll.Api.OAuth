using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Models
{
    /// <summary>
    /// 手机号登录
    /// </summary>
    public class MobileLoginForm
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Required]
        [StringLength(10)]
        public string Code { get; set; }

        /// <summary>
        /// 客户端id
        /// </summary>
        [Required]
        public LoginClientForm Client { get; set; }
    }
}
