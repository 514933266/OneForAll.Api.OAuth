using OAuth.Domain.Enums;
using OAuth.Domain.ValueObjects;
using OAuth.Public.Models;
using OneForAll.Core;
using OneForAll.Core.DDD;
using OneForAll.Core.Extension;
using OneForAll.Core.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace OAuth.Domain.AggregateRoots
{
    /// <summary>
    /// 基础表：用户
    /// </summary>
    public partial class SysUser : AggregateRoot<Guid>
    {
        /// <summary>
        /// 机构Id
        /// </summary>
        [Required]
        public Guid SysTenantId { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [Required]
        [StringLength(32)]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [StringLength(32)]
        public string Password { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Mobile { get; set; } = "";

        /// <summary>
        /// 头像
        /// </summary>
        [StringLength(300)]
        public string IconUrl { get; set; } = "";

        /// <summary>
        /// 个性签名
        /// </summary>
        [StringLength(100)]
        public string Signature { get; set; } = "";

        /// <summary>
        /// 用户状态
        /// </summary>
        [Required]
        public SysUserStatusEnum Status { get; set; }

        /// <summary>
        /// 是否默认（默认用户禁止删除）
        /// </summary>
        [Required]
        public bool IsDefault { get; set; }

        /// <summary>
        /// 最后登陆时间
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 最后登陆Ip
        /// </summary>
        [Column(TypeName = "varchar(50)")]
        public string LastLoginIp { get; set; } = "";

        /// <summary>
        /// 状态最后更新时间（Status）
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 密码输入错误次数
        /// </summary>
        [Required]
        public byte PwdErrCount { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
