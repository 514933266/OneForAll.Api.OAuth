using OneForAll.Core;
using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OAuth.Domain.AggregateRoots
{
    /// <summary>
    /// 基础表：机构（租户）
    /// </summary>
    public partial class SysTenant : AggregateRoot<Guid>, ICreateTime
    {
        public SysTenant()
        {
            SysUsers = new HashSet<SysUser>();
        }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Manager { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Phone { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Required]
        [StringLength(300)]
        public string Address { get; set; }

        /// <summary>
        /// 是否默认（默认机构禁止删除）
        /// </summary>
        [Required]
        public bool IsDefault { get; set; }

        /// <summary>
        /// 是否启用（未启用机构用户禁止登录）
        /// </summary>
        [Required]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Required]
        [StringLength(300)]
        public string Description { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }

        public virtual ICollection<SysUser> SysUsers { get; set; }
    }
}
