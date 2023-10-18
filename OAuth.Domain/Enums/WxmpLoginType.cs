using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Enums
{
    /// <summary>
    /// 微信小程序登录类型
    /// </summary>
    public enum WxmpLoginType
    {
        /// <summary>
        /// 快捷登录
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 手机号登录（）
        /// </summary>
        Phone = 1
    }
}
