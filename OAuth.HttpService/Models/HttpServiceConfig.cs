using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth.HttpService.Models
{
    /// <summary>
    /// 数据资源服务配置
    /// </summary>
    public class HttpServiceConfig
    {
        /// <summary>
        /// 权限验证接口
        /// </summary>
        public string SysPermissionCheck { get; set; } = "SysPermissionCheck";

        /// <summary>
        /// 企业微信通知
        /// </summary>
        public string UmsWechatQyRobot { get; set; } = "UmsWechatQyRobot";

        /// <summary>
        /// 微信小程序-登录
        /// </summary>
        public string WxmpLogin2Session { get; set; } = "WxmpLogin2Session";

        /// <summary>
        /// 微信小程序-Access_token
        /// </summary>
        public string WxmpAccessToken { get; set; } = "WxmpAccessToken";

        /// <summary>
        /// 微信小程序-稳定Access_token
        /// </summary>
        public string WxStableAccessToken { get; set; } = "WxStableAccessToken";

        /// <summary>
        /// 微信小程序-用户手机号
        /// </summary>
        public string WxmpPhoneNumber { get; set; } = "WxmpPhoneNumber";

        /// <summary>
        /// IdentityServer4
        /// </summary>
        public string IdentityServer4Login { get; set; } = "IdentityServer4Login";

        /// <summary>
        /// 腾讯云短信
        /// </summary>
        public string TxCloudSms { get; set; } = "TxCloudSms";

        /// <summary>
        /// 定时任务调度中心
        /// </summary>
        public string ScheduleJob { get; set; } = "ScheduleJob";

        /// <summary>
        /// 全局异常日志
        /// </summary>
        public string SysGlobalExceptionLog { get; set; } = "SysGlobalExceptionLog";

        /// <summary>
        /// Api日志
        /// </summary>
        public string SysApiLog { get; set; } = "SysApiLog";
    }
}
