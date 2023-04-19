using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Application.Dtos
{
    /// <summary>
    /// 网站设置
    /// </summary>
    public class SysWebsiteSettingDto
    {
        public SysWebsiteSettingDto()
        {
            Apis = new HashSet<SysWebsiteApiSettingDto>();
        }

        public Guid Id { get; set; }

        /// <summary>
        /// 网站名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 域名
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 客户端
        /// </summary>
        public string OAuthClientId { get; set; }

        /// <summary>
        /// 客户端密码
        /// </summary>
        public string OAuthClientSecret { get; set; }

        /// <summary>
        /// 客户端名称
        /// </summary>
        public string OAuthClientName { get; set; }

        /// <summary>
        /// 授权地址
        /// </summary>
        public string OAuthUrl { get; set; }

        /// <summary>
        /// 登录背景图地址
        /// </summary>
        public string LoginBackgroundUrl { get; set; }

        /// <summary>
        /// Api明细
        /// </summary>
        public ICollection<SysWebsiteApiSettingDto> Apis { get; set; }
    }
}
