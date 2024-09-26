using OAuth.Domain.Aggregates;
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
    /// 风险账户
    /// </summary>
    public class SysUserRiskRecord
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        /// <summary>
        /// 风险类型
        /// </summary>
        [Required]
        public SysRiskUserTypeEnum Type { get; set; }

        /// <summary>
        /// 风险等级
        /// </summary>
        [Required]
        public int Level { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Remark { get; set; } = "";

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
