using OAuth.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace OAuth.Domain.Models
{
    /// <summary>
    /// 登录提交信息
    /// </summary>
    public class OAuthLoginVo
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

        /// <summary>
        /// 验证码
        /// </summary>
        public string Captcha { get; set; }

        /// <summary>
        /// 验证码秘钥
        /// </summary>
        public string CaptchaKey { get; set; }
    }
}
