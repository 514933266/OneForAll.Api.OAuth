using OAuth.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace OAuth.Domain.Models
{
    /// <summary>
    /// 登录提交信息
    /// </summary>
    public class OAuthLogin
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAddress { get; set; }
    }
}
