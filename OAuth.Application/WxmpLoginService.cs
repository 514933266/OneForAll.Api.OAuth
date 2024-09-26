using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OAuth.Application.Interfaces;
using OAuth.Domain.Interfaces;
using OAuth.Domain.Models;
using OAuth.Domain.ValueObjects;
using OneForAll.Core;
using OneForAll.Core.Extension;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth.Application
{
    /// <summary>
    /// 微信小程序
    /// </summary>
    public class WxmpLoginService : IWxmpLoginService
    {
        private readonly string CACHE_KEY = "LoginCode:{0}";
        private readonly IConfiguration _config;
        private readonly IWxmpLoginManager _manager;
        private readonly IDistributedCache _cacheRepository;

        public WxmpLoginService(
            IConfiguration config,
            IWxmpLoginManager manager,
            IDistributedCache cacheRepository)
        {
            _config = config;
            _manager = manager;
            _cacheRepository = cacheRepository;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>登录结果</returns>
        public async Task<BaseMessage> LoginAsync(WxmpUserLoginForm form)
        {
            #region 测试账号登录

            var msg = new BaseMessage();
            if (!form.PhoneNumber.IsNullOrEmpty())
            {
                // 默认账号
                var rootConfig = new OAuthRootMobileLoginVo();
                _config.GetSection("MobileRootAccount").Bind(rootConfig);
                if (rootConfig != null)
                {
                    var item = rootConfig.Mobiles.FirstOrDefault(w => w == form.PhoneNumber);
                    if (item != null)
                    {
                        if (form.Code != rootConfig.Code)
                            return msg.Fail("验证码错误");

                        return await _manager.LoginAsync(form);
                    }
                }

                var cacheKey = CACHE_KEY.Fmt(form.PhoneNumber);
                var code = await _cacheRepository.GetStringAsync(cacheKey);
                if (code.IsNullOrEmpty() || form.SmsCode != code)
                    return msg.Fail("验证码错误");
                await _cacheRepository.RemoveAsync(cacheKey);
            }
            #endregion

            return await _manager.LoginAsync(form);
        }
    }
}
