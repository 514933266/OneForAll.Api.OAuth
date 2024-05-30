using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.ValueObjects
{
    /// <summary>
    /// 手机号登陆
    /// </summary>
    public class OAuthRootMobileLoginVo
    {
        /// <summary>
        /// 手机号/账号
        /// </summary>
        public List<string> Mobiles { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }
    }
}
