using AutoMapper;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.IdentityModel.Tokens;
using OAuth.Domain.AggregateRoots;
using OAuth.Domain.Enums;
using OAuth.Domain.Interfaces;
using OAuth.Domain.Repositorys;
using OAuth.Domain.ValueObjects;
using OneForAll.Core;
using OneForAll.Core.DDD;
using OneForAll.Core.Extension;
using OneForAll.Core.Security;
using OneForAll.Core.Utility;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain
{
    /// <summary>
    /// 微信登录用户
    /// </summary>
    public class SysWxUserManager : BaseManager, ISysWxUserManager
    {
        private readonly IMapper _mapper;
        private readonly ISysWxUserRepository _repository;
        private readonly ISysUserRepository _userRepository;
        public SysWxUserManager(
            IMapper mapper,
            ISysWxUserRepository repository,
            ISysUserRepository userRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// 获取已登录过得用户
        /// </summary>
        /// <param name="appId">应用id</param>
        /// <param name="unionId">用户在开放平台的唯一标识符，若当前小程序已绑定到微信开放平台账号下会返回</param>
        /// <returns></returns>
        public async Task<SysWxUser> GetAsync(string appId, string unionId)
        {
            return await _repository.GetAsync(w => w.AppId == appId && w.UnionId == unionId);
        }

        /// <summary>
        /// 获取对应的微信用户
        /// </summary>
        /// <param name="appId">应用id</param>
        /// <param name="userId">系统登录用户id</param>
        /// <returns></returns>
        public async Task<SysWxUser> GetAsync(string appId, Guid userId)
        {
            return await _repository.GetAsync(w => w.AppId == appId && w.SysUserId == userId);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="user">系统用户</param>
        /// <param name="role">登录的角色</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> LoginAsync(SysWxUser entity, SysUser user, string role)
        {
            var data = await _repository.GetAsync(w => w.AppId == entity.AppId && w.OpenId == entity.OpenId);

            if (data == null)
            {
                if (user != null)
                {
                    entity.SysUserId = user.Id;
                    return await ResultAsync(() => _repository.AddAsync(entity));
                }
            }
            else
            {
                data.SysUserId = user.Id;
                data.SessionKey = entity.SessionKey;
                data.AccessToken = entity.AccessToken;
                data.AccessTokenCreateTime = entity.AccessTokenCreateTime;
                data.AccessTokenExpiresIn = entity.AccessTokenExpiresIn;
                if (data.Mobile != entity.Mobile && !entity.Mobile.IsNullOrEmpty())
                {
                    data.Mobile = entity.Mobile;
                }
                return await ResultAsync(() => _repository.UpdateAsync(data));
            }
            return BaseErrType.Fail;
        }
    }
}
