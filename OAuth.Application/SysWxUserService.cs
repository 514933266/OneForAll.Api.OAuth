using AutoMapper;
using Microsoft.Extensions.Configuration;
using OAuth.Application.Dtos;
using OAuth.Application.Interfaces;
using OAuth.Domain.Interfaces;
using OAuth.Domain.Repositorys;
using OAuth.HttpService.Interfaces;
using OneForAll.Core;
using OneForAll.EFCore;
using SysLog.HttpService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Application
{
    /// <summary>
    /// 微信用户
    /// </summary>
    public class SysWxUserService : ISysWxUserService
    {
        private readonly IMapper _mapper;
        private readonly ISysWxUserRepository _repository;
        private readonly ISysWxClientRepository _clientRepository;
        public SysWxUserService(
            IMapper mapper,
            ISysWxUserRepository repository,
            ISysWxClientRepository clientRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _clientRepository = clientRepository;
        }

        /// <summary>
        /// 获取微信用户基础信息
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="clientId">客户端id</param>
        /// <returns></returns>
        public async Task<SysWxUserBaseDto> GetAsync(Guid userId, string clientId)
        {
            var setting = await _clientRepository.GetByClientIdAsync(clientId);
            if (setting == null)
                return new SysWxUserBaseDto();

            var user = await _repository.GetAsync(w => w.SysUserId == userId && w.AppId == setting.AppId);
            if (user == null)
                return new SysWxUserBaseDto();
            return _mapper.Map<SysWxUserBaseDto>(user);
        }
    }
}
