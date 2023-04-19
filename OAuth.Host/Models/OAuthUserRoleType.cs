namespace OAuth.Host.Models
{
    /// <summary>
    /// 用户角色类型
    /// </summary>
    public class OAuthUserRoleType
    {
        public const string RULER = "ruler";

        public const string ADMIN = "admin";

        public const string DEVELOPER = "developer";

        public const string PUBLIC = "ruler,admin";
    }
}
