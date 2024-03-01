using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.HttpService.Models
{
    /// <summary>
    /// API日志
    /// </summary>
    public class SysApiLogRequest
    {
        /// <summary>
        /// 租户id
        /// </summary>
        [Required]
        public Guid TenantId { get; set; }

        /// <summary>
        /// 所属模块名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string MoudleName { get; set; }

        /// <summary>
        /// 模块代码
        /// </summary>
        [Required]
        [StringLength(50)]
        public string MoudleCode { get; set; }

        /// <summary>
        /// 控制器
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Controller { get; set; }

        /// <summary>
        /// 控制器方法
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Action { get; set; }

        /// <summary>
        /// 请求域名
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Host { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        [Required]
        [StringLength(1000)]
        public string Url { get; set; }

        /// <summary>
        /// 请求方法：GET、POST、PUT、DELETE等
        /// </summary>
        [Required]
        [StringLength(10)]
        public string Method { get; set; }

        /// <summary>
        /// 请求类型，如果是文件类型
        /// </summary>
        [Required]
        [StringLength(50)]
        public string ContentType { get; set; }

        /// <summary>
        /// 请求Body内容，如果是文件上传类不记录
        /// </summary>
        [Required]
        public string RequestBody { get; set; }

        /// <summary>
        /// 响应内容，如果是下载不记录
        /// </summary>
        [Required]
        public string ReponseBody { get; set; }

        /// <summary>
        /// 完整的浏览器信息
        /// </summary>
        [Required]
        [StringLength(200)]
        public string UserAgent { get; set; }

        /// <summary>
        /// Ip地址
        /// </summary>
        [Required]
        [StringLength(50)]
        public string IPAddress { get; set; }

        /// <summary>
        /// 消耗时间
        /// </summary>
        [Required]
        [StringLength(20)]
        public string TimeSpan { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        [Required]
        [StringLength(10)]
        public string StatusCode { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        [Required]
        public Guid CreatorId { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        [Required]
        [StringLength(20)]
        public string CreatorName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
