using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Models
{
    /// <summary>
    /// 腾讯云短信消息
    /// </summary>
    public class TxCloudSmsForm
    {
        /// <summary>
        /// 短信签名
        /// </summary>
        [StringLength(10)]
        public string SignName { get; set; }

        /// <summary>
        /// 模板id
        /// </summary>
        [Required]
        [StringLength(20)]
        public string TemplateId { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 短信内容,70个字
        /// </summary>
        [Required]
        [StringLength(140)]
        public string Content { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        [Required]
        public string Sign { get; set; }
    }
}
