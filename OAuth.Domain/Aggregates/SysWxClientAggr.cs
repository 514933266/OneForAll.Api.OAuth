using OAuth.Domain.AggregateRoots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Aggregates
{
    /// <summary>
    /// 微信客户端
    /// </summary>
    public class SysWxClientAggr : SysWxClient
    {
        /// <summary>
        /// 系统客户端
        /// </summary>
        public SysClient SysClient { get; set; }
    }
}
