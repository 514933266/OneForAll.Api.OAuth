namespace OAuth.Domain.ValueObjects
{
    /// <summary>
    /// 登录配置
    /// </summary>
    public class OAuthLoginSetting
    {
        /// <summary>
        /// 冻结时间
        /// </summary>
        public int BanTime { get; set; }

        /// <summary>
        /// 最大错误次数
        /// </summary>
        public int MaxPwdErrCount { get; set; }
    }
}
