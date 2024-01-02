using OAuth.Application.Interfaces;
using OAuth.Domain.Interfaces;
using OAuth.Domain.Models;
using OneForAll.Core;
using System.Threading.Tasks;

namespace OAuth.Application
{
    /// <summary>
    /// 微信小程序
    /// </summary>
    public class WxmpLoginService : IWxmpLoginService
    {
        private readonly IWxmpLoginManager _manager;

        public WxmpLoginService(IWxmpLoginManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>登录结果</returns>
        public async Task<BaseMessage> LoginAsync(WxmpUserLoginForm form)
        {
            return await _manager.LoginAsync(form);
        }
    }
}
