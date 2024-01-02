using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Enums
{
    /// <summary>
    /// 客户端类型
    /// </summary>
    public enum SysClientTypeEnum
    {
        /// <summary>
        /// 网站
        /// </summary>
        Web = 1000,

        /// <summary>
        /// App
        /// </summary>
        App = 2000,

        /// <summary>
        /// 微信公众号
        /// </summary>
        Wxgzh = 3000,

        /// <summary>
        /// 微信小程序
        /// </summary>
        Wxmp = 3001,
    }
}
