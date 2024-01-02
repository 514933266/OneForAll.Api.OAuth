using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using OAuth.Application.Interfaces;
using OAuth.Domain;
using OAuth.Domain.AggregateRoots;
using OAuth.Domain.Aggregates;
using OAuth.Domain.Enums;
using OAuth.Domain.Interfaces;
using OAuth.Domain.Models;
using OAuth.Domain.Repositorys;
using OAuth.HttpService;
using OAuth.HttpService.Interfaces;
using OAuth.HttpService.Models;
using OAuth.Public.Models;
using OneForAll.Core;
using OneForAll.Core.Extension;
using OneForAll.Core.Security;
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
        private readonly IIdentityServer4HttpService _identityHttpService;
        private readonly IMobileLoginManager _manager;
        private readonly ITxCloudSmsManager _smsManager;
        private readonly IDistributedCache _cacheRepository;

        public MobileLoginService(
            IConfiguration config,
            IIdentityServer4HttpService identityHttpService,
            IMobileLoginManager manager,
            ITxCloudSmsManager smsManager,
            IDistributedCache cacheRepository)
        {
            _config = config;
            _manager = manager;
            _smsManager = smsManager;
            _identityHttpService = identityHttpService;
            _cacheRepository = cacheRepository;
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="phoneNumber">手机号码</param>
        /// <returns>登录结果</returns>
        public async Task<BaseErrType> SendCodeAsync(string phoneNumber)
        {
            var cacheKey = CACHE_KEY.Fmt(phoneNumber);
            var templateId = _config["TxCloudSms:TemplateId"];
            var signName = _config["TxCloudSms:SignName"];
            var code = StringHelper.GetRandomNumber(4);
            try
            {
                var exists = await _cacheRepository.GetStringAsync(cacheKey);
                if (!exists.IsNullOrEmpty())
                    return BaseErrType.DataExist;

                await _cacheRepository.SetStringAsync(cacheKey, code, new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60)
                });
            }
            catch
            {
                throw new Exception("Redis服务未启动！");
            }

            var sign = (code + DateTime.Now.ToString("yyyyMMddhhmm")).ToMd5();
            return await _smsManager.SendAsync(new TxCloudSmsForm()
            {
                SignName = signName,
                Content = code,
                TemplateId = templateId,
                PhoneNumber = phoneNumber,
                Sign = sign
            });
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        public async Task<BaseMessage> LoginAsync(MobileLoginForm form)
        {
            // 默认账号/验证码校验
            var msg = new BaseMessage();
            var rootAcc = _config["MobileRootAccount:UserName"];
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

            return await _manager.LoginAsync(form);
        }
    }
}
