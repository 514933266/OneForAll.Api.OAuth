using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.AggregateRoots
{
    /// <summary>
    /// 系统客户端-微信客户端关联表
    /// </summary>
    public class SysWxClientContact
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 系统客户端id
        /// </summary>
        [Required]
        public Guid SysClientId { get; set; }

        /// <summary>
        /// 微信客户端id
        /// </summary>
        [Required]
        public Guid SysWxClientId { get; set; }
    }
}
