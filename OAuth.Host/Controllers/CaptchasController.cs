using Microsoft.AspNetCore.Mvc;
using OAuth.Application.Interfaces;
using OneForAll.Core;
using System.IO;
using System.Threading.Tasks;

namespace OAuth.Host.Controllers
{
    /// <summary>
    /// 手机号登录
    /// </summary>
    [Route("api/[controller]")]
    public class CaptchasController : Controller
    {
        private readonly ICaptchaService _service;
        public CaptchasController(ICaptchaService service)
        {
            _service = service;
        }

        /// <summary>
        /// 获取Base64验证码
        /// </summary>
        /// <param name="key">秘钥</param>
        /// <returns>实体</returns>
        [HttpGet]
        [Route("ImageBase64")]
        public async Task<string> GetImageAsync([FromQuery] string key)
        {
            return await _service.GetImageBase64Async(key);
        }
    }
}
