using OAuth.Domain.AggregateRoots;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Interfaces
{
    /// <summary>
    /// 微信登录用户
    /// </summary>
    public interface ISysWxUserManager
    {
        /// <summary>
        /// 获取已登录过得用户
        /// </summary>
        /// <param name="appId">应用id</param>
        /// <param name="unionId">用户在开放平台的唯一标识符，若当前小程序已绑定到微信开放平台账号下会返回</param>
        /// <returns></returns>
        Task<SysWxUser> GetAsync(string appId, string unionId);

        /// <summary>
        /// 获取对应的微信用户
        /// </summary>
        /// <param name="appId">应用id</param>
        /// <param name="userId">系统登录用户id</param>
        /// <returns></returns>
        Task<SysWxUser> GetAsync(string appId, Guid userId);

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="user">系统用户</param>
        /// <param name="role">登录的角色</param>
        /// <returns>结果</returns>
        Task<BaseErrType> LoginAsync(SysWxUser entity, SysUser user, string role);
    }
}
