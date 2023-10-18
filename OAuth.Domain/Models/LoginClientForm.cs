using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Models
{
    /// <summary>
    /// 客户端信息
    /// </summary>
    public class LoginClientForm
    {
        /// <summary>
        /// 客户端id
        /// </summary>
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// 客户端密码
        /// </summary>
        [Required]
        public string Secret { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        [Required]
        public string Role { get; set; }
    }
}
