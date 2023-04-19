using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Application.Dtos
{
    /// <summary>
    /// 网站设置-Api
    /// </summary>
    public class SysWebsiteApiSettingDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 请求的域名
        /// </summary>
        public string Host { get; set; }
    }
}
