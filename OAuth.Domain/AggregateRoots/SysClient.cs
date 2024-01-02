using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OAuth.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.AggregateRoots
{
    /// <summary>
    /// 客户端
    /// </summary>
    public class SysClient
    {
        /// <summary>
        /// 实体id
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 系统客户端id
        /// </summary>
        [Required]
        [StringLength(100)]
        public string ClientId { get; set; }

        /// <summary>
        /// 系统客户端密码
        /// </summary>
        [Required]
        [StringLength(100)]
        public string ClientSecret { get; set; }

        /// <summary>
        /// 系统客户端名称
        /// </summary>
        [Required]
        [StringLength(20)]
        public string ClientName { get; set; }

        /// <summary>
        /// 允许自动创建账号
        /// </summary>
        [Required]
        public bool AutoCreateAccount { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [Required]
        public SysClientTypeEnum Type { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Role { get; set; } = "admin";

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
