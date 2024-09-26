using Microsoft.Extensions.Caching.Distributed;
using OAuth.Domain.Interfaces;
using OAuth.Public;
using OAuth.Public.Models;
using OneForAll.Core.Extension;
using OneForAll.Core.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain
{
    /// <summary>
    /// 验证码
    /// </summary>
    public class CaptchaManager : ICaptchaManager
    {
        private readonly IDistributedCache _cacheRepository;
        public CaptchaManager(IDistributedCache cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }

        /// <summary>
        /// 设置验证码
        /// </summary>
        /// <param name="codeKey"></param>
        /// <returns></returns>
        public async Task<string> SetStrCodeAsync(string codeKey)
        {
            var cacheKey = ConstRedisKey.VerificationCode.Fmt(codeKey);
            var codeStr = StringHelper.GetRandomLetter(4);
            var exists = await _cacheRepository.GetStringAsync(cacheKey);
            if (!exists.IsNullOrEmpty())
                await _cacheRepository.RemoveAsync(cacheKey);

            await _cacheRepository.SetStringAsync(cacheKey, codeStr, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });

            return codeStr;
        }

        /// <summary>
        /// 检查是否存在验证码
        /// </summary>
        /// <param name="codeKey">账号</param>
        /// <returns></returns>
        public async Task<bool> ExistsStrCodeAsync(string codeKey)
        {
            var cacheKey = ConstRedisKey.VerificationCode.Fmt(codeKey);
            var exists = await _cacheRepository.GetStringAsync(cacheKey);
            return !exists.IsNullOrEmpty();
        }

        /// <summary>
        /// 检查验证码
        /// </summary>
        /// <param name="codeKey">秘钥</param>
        /// <param name="code">输入的验证码</param>
        /// <returns></returns>
        public async Task<bool> CheckStrCodeAsync(string codeKey, string code)
        {
            var pass = false;
            if (!code.IsNullOrEmpty())
            {
                var cacheKey = ConstRedisKey.VerificationCode.Fmt(codeKey);
                var exists = await _cacheRepository.GetStringAsync(cacheKey);
                if (!exists.IsNullOrEmpty())
                    pass = exists.ToLower() == code.ToLower();
            }
            return pass;
        }

        /// <summary>
        /// 获取Base64验证码
        /// </summary>
        /// <param name="codeKey">登录账号</param>
        /// <returns>结果</returns>
        public async Task<string> GetStrCodeBase64Async(string codeKey)
        {
            var codeStr = await SetStrCodeAsync(codeKey);
            return new CaptchaHelper().GetImageBase64(codeStr);
        }
    }
}
