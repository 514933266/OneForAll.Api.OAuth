using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using OAuth.Domain.AggregateRoots;
using OAuth.Domain.Interfaces;
using OAuth.Domain.Models;
using OAuth.Domain.Repositorys;
using OneForAll.Core;
using OneForAll.Core.DDD;
using OneForAll.Core.Extension;
using OneForAll.Core.Security;
using OneForAll.Core.Utility;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain
{
    /// <summary>
    /// 手机号登录
    /// </summary>
    public class MobileLoginManager : BaseManager, IMobileLoginManager
    {
        private readonly IMapper _mapper;
        private readonly ISysUserRepository _repository;
        private readonly ISysWechatUserRepository _wxUserRepository;

        public MobileLoginManager(
            IMapper mapper,
            ISysUserRepository repository,
            ISysWechatUserRepository wxUserRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _wxUserRepository = wxUserRepository;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        public async Task<SysUser> LoginAsync(MobileLoginForm form)
        {
            var data = await _repository.GetAsync(w => w.UserName == form.PhoneNumber);

            var user = new SysUser()
            {
                UserName = form.PhoneNumber,
                Password = StringHelper.GetRandomString(12).ToMd5(),
                Name = "手机用户",
                Status = (int)BaseErrType.Success
            };

            if (data == null)
            {
                // 没有当前的手机号码，查找微信账号是否存在对应的手机号码
                var wxUser = await _wxUserRepository.GetAsync(w => w.Mobile == form.PhoneNumber);
                if (wxUser != null)
                {
                    return await _repository.GetAsync(w => w.Id == wxUser.SysUserId);
                }
                await ResultAsync(() => _repository.AddAsync(data));
                return data;
            }
            else
            {
                return data;
            }
        }
    }
}
