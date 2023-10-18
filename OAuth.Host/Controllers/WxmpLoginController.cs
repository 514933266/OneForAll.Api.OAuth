using Microsoft.AspNetCore.Mvc;
using OAuth.Application.Dtos;
using OAuth.Application.Interfaces;
using OAuth.Domain.Models;
using OneForAll.Core;
using System.Threading.Tasks;

namespace OAuth.Host.Controllers
{
    /// <summary>
    /// 微信小程序登录
    /// </summary>
    [Route("api/[controller]")]
    public class WxmpLoginController : Controller
    {
        private readonly IWxmpLoginService _service;
        public WxmpLoginController(IWxmpLoginService service)
        {
            _service = service;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>实体</returns>
        [HttpPost]
        public async Task<BaseMessage> LoginAsync([FromBody] WxmpUserLoginForm form)
        {
            return await _service.LoginAsync(form);
        }
    }
}
