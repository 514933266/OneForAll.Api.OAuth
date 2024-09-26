using OAuth.Domain.AggregateRoots;
using OAuth.Domain.Models;
using OAuth.Public.Models;
using OneForAll.Core;
using OneForAll.Core.OAuth;

namespace OAuth.Domain.ValueObjects
{
    /// <summary>
    /// 登录结果
    /// </summary>
    public class OAuthLoginResultVo
    {
        /// <summary>
        /// 错误类型
        /// </summary>
        public BaseErrType ErrType { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public LoginUser User { get; set; }

        /// <summary>
        /// 租户信息
        /// </summary>
        public SysTenant SysTenant { get; set; }

        /// <summary>
        /// 剩余密码可错误次数
        /// </summary>
        public int LessPwdErrCount { get; set; }

        /// <summary>
        /// 剩余冻结时间
        /// </summary>
        public double LessBanTime { get; set; }

        /// <summary>
        /// 需要提交验证码
        /// </summary>
        public bool IsRequiredCaptcha { get; set; }
    }
}
