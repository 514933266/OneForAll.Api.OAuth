using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Public.Models
{
    /// <summary>
    /// 系统登录用户
    /// </summary>
    public class LoginUser
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 所属机构Id
        /// </summary>
        public Guid TenantId { get; set; }

        /// <summary>
        /// 对应OA档案id
        /// </summary>
        public Guid PersonId { get; set; }

        /// <summary>
        /// 机构是否默认
        /// </summary>
        public bool IsDefaultTenant { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否默认（默认用户不可删除）
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Api权限角色
        /// </summary>
        public string ApiRole { get; set; }

        /// <summary>
        /// 微信登录端id
        /// </summary>
        public string WxAppId { get; set; }

        /// <summary>
        /// 微信登录端openid
        /// </summary>
        public string WxOpenId { get; set; }

        /// <summary>
        /// 微信登录端unionid
        /// </summary>
        public string WxUnionId { get; set; }
    }
}
