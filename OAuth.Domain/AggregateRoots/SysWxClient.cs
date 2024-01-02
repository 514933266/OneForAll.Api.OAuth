using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    /// 微信客户端
    /// </summary>
    public class SysWxClient
    {
        /// <summary>
        /// 实体id
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 微信客户端id
        /// </summary>
        [Required]
        [StringLength(100)]
        public string AppId { get; set; }

        /// <summary>
        /// 微信客户端密码
        /// </summary>
        [Required]
        [StringLength(100)]
        public string AppSecret { get; set; }

        /// <summary>
        /// 微信的accessToken
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

    }
}
