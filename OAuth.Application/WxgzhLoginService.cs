using AutoMapper;
using Microsoft.Extensions.Configuration;
using OAuth.Application.Interfaces;
using OAuth.Domain.Interfaces;
using OAuth.Domain.Repositorys;
using OneForAll.Core;
using SysLog.HttpService.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Application
{
    /// <summary>
    /// 微信公众号登录
    /// </summary>
    public class WxgzhLoginService : IWxgzhLoginService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IWxmpHttpService _httpService;
        private readonly IIdentityServer4HttpService _identityHttpService;
        private readonly ISysWechatUserManager _manager;
        private readonly ISysUserRepository _userRepository;
        public WxgzhLoginService(
            IMapper mapper,
            IConfiguration config,
            IWxmpHttpService httpService,
            IIdentityServer4HttpService identityHttpService,
            ISysWechatUserManager manager,
            ISysUserRepository userRepository)
        {
            _mapper = mapper;
            _config = config;
            _manager = manager;
            _httpService = httpService;
            _identityHttpService = identityHttpService;
            _userRepository = userRepository;
        }
    }
}
