using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Aggregates
{
    /// <summary>
    /// 风险异常类型
    /// </summary>
    public enum SysRiskUserTypeEnum
    {
        /// <summary>
        /// 密码输入错误次数过多
        /// </summary>
        [Description("密码输入错误次数过多")]
        PwdError = 1000
    }
}
