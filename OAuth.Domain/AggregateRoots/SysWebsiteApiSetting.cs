using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.AggregateRoots
{
    /// <summary>
    /// 网站设置-Api
    /// </summary>
    public class SysWebsiteApiSetting : AggregateRoot<Guid>
    {
        /// <summary>
        /// 网站设置Id
        /// </summary>
        [Required]
        public Guid SysWebsiteSettingId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Code { get; set; }

        /// <summary>
        /// 请求的域名
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Host { get; set; }
    }
}
