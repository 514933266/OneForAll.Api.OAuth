using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OAuth.Domain.AggregateRoots
{
    /// <summary>
    /// 微信用户
    /// </summary>
    public class SysWxUser
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 基础用户id
        /// </summary>
        [Required]
        public Guid SysUserId { get; set; }

        /// <summary>
        /// 应用id
        /// </summary>
        [Required]
        [StringLength(200)]
        public string AppId { get; set; }

        /// <summary>
        /// OpenId：每个应用唯一
        /// </summary>
        [Required]
        [StringLength(200)]
        public string OpenId { get; set; }

        /// <summary>
        /// 会话密钥
        /// </summary>
        [Required]
        [StringLength(200)]
        public string SessionKey { get; set; } = "";

        /// <summary>
        /// UnionId
        /// </summary>
        [Required]
        [StringLength(200)]
        public string UnionId { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string NickName { get; set; } = "";

        /// <summary>
        /// 手机号码
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Mobile { get; set; } = "";

        /// <summary>
        /// 头像
        /// </summary>
        [Required]
        [StringLength(2000)]
        public string AvatarUrl { get; set; } = "";

        /// <summary>
        /// 访问凭证
        /// </summary>
        [Required]
        [StringLength(200)]
        public string AccessToken { get; set; } = "";

        /// <summary>
        /// 访问凭证过期时间
        /// </summary>
        [Required]
        public int AccessTokenExpiresIn { get; set; }

        /// <summary>
        /// 访问凭证获取时间
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? AccessTokenCreateTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
