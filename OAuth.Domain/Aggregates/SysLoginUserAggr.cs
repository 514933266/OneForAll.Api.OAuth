using OAuth.Domain.AggregateRoots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Aggregates
{
    /// <summary>
    /// 系统用户
    /// </summary>
    public class SysLoginUserAggr : SysUser
    {
        /// <summary>
        /// 所属机构
        /// </summary>
        public SysTenant SysTenant { get; set; }

        /// <summary>
        /// 微信用户信息
        /// </summary>
        public SysWechatUser SysWechatUser { get; set; }
    }
}
