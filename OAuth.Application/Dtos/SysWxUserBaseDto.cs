using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Application.Dtos
{
    /// <summary>
    /// 微信登录公开信息
    /// </summary>
    public class SysWxUserBaseDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 基础用户id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// OpenId：每个应用唯一
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// 访问凭证
        /// </summary>
        public string AccessToken { get; set; }
    }
}
