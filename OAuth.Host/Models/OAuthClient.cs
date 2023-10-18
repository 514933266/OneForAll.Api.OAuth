using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth.Host.Models
{
    /// <summary>
    /// 实体：授权客户端
    /// </summary>
    public class OAuthClient
    {
        /// <summary>
        /// 客户端id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 客户端密码
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// 客户端名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// 范围
        /// </summary>
        public List<string> Scopes { get; set; } = new List<string>();

        /// <summary>
        /// 是否微信客户端
        /// </summary>
        public bool IsWechat { get; set; }
    }
}
