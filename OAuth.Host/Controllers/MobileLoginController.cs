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
        public async Task<BaseMessage> SendCodeAsync([FromBody] string phoneNumber)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _service.SendCodeAsync(phoneNumber);
            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("发送成功");
                case BaseErrType.DataExist: return msg.Fail("短信已发送，请1分钟后再重新获取");
                default: return msg.Fail("发送失败");
            }
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
