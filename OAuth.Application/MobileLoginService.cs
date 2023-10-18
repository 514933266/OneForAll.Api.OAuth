using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using OAuth.Application.Interfaces;
using OAuth.Domain;
using OAuth.Domain.AggregateRoots;
using OAuth.Domain.Interfaces;
using OAuth.Domain.Models;
using OAuth.Domain.Repositorys;
using OAuth.HttpService.Interfaces;
using OAuth.HttpService.Models;
using OneForAll.Core;
using OneForAll.Core.Extension;
using OneForAll.Core.Utility;
using SysLog.HttpService.Interfaces;
using SysLog.HttpService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Application
{
    /// <summary>
    /// 手机号登录
    /// </summary>
    internal class MobileLoginService : IMobileLoginService
    {
        private readonly string CACHE_KEY = "LoginCode:{0}";

        private readonly IConfiguration _config;
        private readonly ITxCloudSmsHttpService _httpService;
        private readonly IIdentityServer4HttpService _identityHttpService;
        private readonly IMobileLoginManager _manager;
        private readonly IDistributedCache _cacheRepository;

        public MobileLoginService(
            IConfiguration config,
            ITxCloudSmsHttpService httpService,
            IIdentityServer4HttpService identityHttpService,
            IMobileLoginManager manager,
            IDistributedCache cacheRepository)
        {
            _config = config;
            _manager = manager;
            _httpService = httpService;
            _identityHttpService = identityHttpService;
            _cacheRepository = cacheRepository;
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="phoneNumber">手机号码</param>
        /// <returns>登录结果</returns>
        public async Task<BaseMessage> GetCodeAsync(string phoneNumber)
        {
            var msg = new BaseMessage();
            var cacheKey = CACHE_KEY.Fmt(phoneNumber);
            var templateId = _config["TxCloudSms:TemplateId"];
            var signName = _config["TxCloudSms:SignName"];
            var code = StringHelper.GetRandomNumber(4);
            try
            {
                var exists = await _cacheRepository.GetStringAsync(cacheKey);
                if (!exists.IsNullOrEmpty())
                    return msg.Fail("验证码已发送，请稍后重新获取！");

                await _cacheRepository.SetStringAsync(cacheKey, code, new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60)
                });
            }
            catch
            {
                throw new Exception("Redis服务未启动！");
            }
            return await _httpService.SendAsync(signName, code, templateId, phoneNumber);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        public async Task<BaseMessage> LoginAsync(MobileLoginForm form)
        {
            var msg = new BaseMessage();
            var rootAcc = _config["MobileRootAccount:UserName"];

            // 默认账号/验证码校验
            if (form.PhoneNumber == rootAcc && !rootAcc.IsNullOrEmpty())
            {
                var code = _config["MobileRootAccount:Code"];
                if (form.Code != code)
                    return msg.Fail("验证码错误");
            }
            else
            {
                var cacheKey = CACHE_KEY.Fmt(form.PhoneNumber);
                var code = await _cacheRepository.GetStringAsync(cacheKey);
                if (code.IsNullOrEmpty() || form.Code != code)
                    return msg.Fail("验证码错误");
                // 删除验证码缓存
                await _cacheRepository.RemoveAsync(cacheKey);
            }

            var user = await _manager.LoginAsync(form);
            var tokenResponse = await _identityHttpService.LoginAsync(new IdentityServer4Request()
            {
                ClientId = form.Client.Id,
                ClientSecret = form.Client.Secret,
                GrantType = "password",
                Username = user.UserName,
                Password = user.Password
            });

            if (!tokenResponse.AccessToken.IsNullOrEmpty())
            {
                msg.Status = true;
                msg.Data = tokenResponse;
            }
            else if (!tokenResponse.Error.IsNullOrEmpty())
            {
                msg.ErrType = BaseErrType.Fail;
                msg.Status = false;
                msg.Message = tokenResponse.Error;
            }
            else
            {
                msg = tokenResponse.Result as BaseMessage;
            }

            return msg;
        }
    }
}
