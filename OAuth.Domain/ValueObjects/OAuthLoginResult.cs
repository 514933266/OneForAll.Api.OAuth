using OAuth.Domain.AggregateRoots;
using OAuth.Domain.Models;
using OneForAll.Core;

namespace OAuth.Domain.ValueObjects
{
    /// <summary>
    /// 登录结果
    /// </summary>
    public class OAuthLoginResult
    {
        /// <summary>
        /// 错误类型
        /// </summary>
        public BaseErrType ErrType { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public OAuthLoginUser User { get; set; }

        /// <summary>
        /// 剩余密码可错误次数
        /// </summary>
        public int LessPwdErrCount { get; set; }

        /// <summary>
        /// 剩余冻结时间
        /// </summary>
        public double LessBanTime { get; set; }
    }
}
