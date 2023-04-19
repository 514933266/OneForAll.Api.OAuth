namespace OAuth.Host.Models
{
    /// <summary>
    /// 跨域配置模型
    /// </summary>
    public class CorsConfig
    {
        /// <summary>
        /// 域名集合
        /// </summary>
        public string[] Origins { get; set; }
    }
}
