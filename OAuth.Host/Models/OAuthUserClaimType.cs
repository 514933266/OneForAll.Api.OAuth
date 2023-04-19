namespace OAuth.Host.Models
{
    /// <summary>
    /// 身份信息类型
    /// </summary>
    public class OAuthUserClaimType
    {
        /// <summary>
        /// 租户id
        /// </summary>
        public const string TENANT_ID = "TenantId";

        /// <summary>
        /// 用户名
        /// </summary>
        public const string USERNAME = "UserName";

        /// <summary>
        /// 名称
        /// </summary>
        public const string USER_NICKNAME = "UserNickName";

        /// <summary>
        /// id
        /// </summary>
        public const string USER_ID = "UserId";

        /// <summary>
        /// 角色
        /// </summary>
        public const string ROLE = "Role";

        /// <summary>
        /// 人员档案id
        /// </summary>
        public const string PERSON_ID = "PersonId";

        /// <summary>
        /// 是否默认用户
        /// </summary>
        public const string IS_DEFAULT = "IsDefault";
    }
}
