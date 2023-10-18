using Microsoft.AspNetCore.Mvc;
using OAuth.Application.Interfaces;
using OAuth.Domain.Models;
using OneForAll.Core;
using System.Threading.Tasks;

namespace OAuth.Host.Controllers
{
    /// <summary>
    /// 手机号登录
    /// </summary>
    [Route("api/[controller]")]
    public class MobileLoginController : Controller
    {
        private readonly IMobileLoginService _service;
        public MobileLoginController(IMobileLoginService service)
        {
            _service = service;
        }

        /// <summary>
        /// 获取登录验证码
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <returns>实体</returns>
        [HttpPost]
        [Route("Code")]
        public async Task<BaseMessage> GetCodeAsync([FromBody] string phoneNumber)
        {
            return await _service.GetCodeAsync(phoneNumber);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>实体</returns>
        [HttpPost]
        public async Task<BaseMessage> LoginAsync([FromBody] MobileLoginForm form)
        {
            return await _service.LoginAsync(form);
        }
    }
}
