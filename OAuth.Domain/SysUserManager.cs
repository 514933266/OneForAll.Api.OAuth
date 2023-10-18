using AutoMapper;
using Microsoft.AspNetCore.Http;
using OAuth.Domain.AggregateRoots;
using OAuth.Domain.Aggregates;
using OAuth.Domain.Interfaces;
using OAuth.Domain.Repositorys;
using OneForAll.Core;
using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain
{
    /// <summary>
    /// 系统用户
    /// </summary>
    public class SysUserManager : BaseManager, ISysUserManager
    {
        private readonly ISysUserRepository _userRepository;
        public SysUserManager(ISysUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddAsync(SysUser entity)
        {
            var data = await _userRepository.GetAsync(w => w.UserName == entity.UserName);
            if (data != null)
                return BaseErrType.DataExist;
            return await ResultAsync(() => _userRepository.AddAsync(data));
        }
    }
}