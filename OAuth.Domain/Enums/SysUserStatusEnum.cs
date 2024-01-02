using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Enums
{
    /// <summary>
    /// 用户状态
    /// </summary>
    public enum SysUserStatusEnum
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 1,

        /// <summary>
        /// 冻结（暂时无法登录30分钟）
        /// </summary>
        Frozen = 2001,

        /// <summary>
        /// 冻结（暂时无法登录1小时）
        /// </summary>
        FrozenOneHour = 2002,

        /// <summary>
        /// 冻结（暂时无法登录3小时）
        /// </summary>
        FrozenThreeHour = 2003,

        /// <summary>
        /// 冻结（暂时无法登录1天）
        /// </summary>
        FrozenDay = 2004,

        /// <summary>
        /// 冻结（暂时无法登录3天）
        /// </summary>
        FrozenThreeDay = 2005,

        /// <summary>
        /// 冻结（暂时无法登录7天）
        /// </summary>
        FrozenSevenDay = 2006,

        /// <summary>
        /// 冻结（暂时无法登录1个月）
        /// </summary>
        FrozenMonth = 2007,

        /// <summary>
        /// 冻结（暂时无法登录3个月）
        /// </summary>
        FrozenThreeMonth = 2008,

        /// <summary>
        /// 封禁（永久禁止登录）
        /// </summary>
        Banned = 2099,

        /// <summary>
        /// 异常
        /// </summary>
        Error = 99
    }
}