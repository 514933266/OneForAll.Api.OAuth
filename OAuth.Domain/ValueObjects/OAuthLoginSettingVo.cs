namespace OAuth.Domain.ValueObjects
{
    /// <summary>
    /// 登录配置
    /// </summary>
    public class OAuthLoginSettingVo
    {
        /// <summary>
        /// 冻结时间
        /// </summary>
        public int BanTime { get; set; }

        /// <summary>
        /// 最大错误次数
        /// </summary>
        public int MaxPwdErrCount { get; set; }

        /// <summary>
        /// 启用验证码
        /// </summary>
        public bool IsCaptchaEnabled { get; set; }

        /// <summary>
        /// 忽略验证码检查的Key，携带此Key请求则不会校验验证码
        /// </summary>
        public string IgnoreCaptchaKey { get; set; }
    }
}
