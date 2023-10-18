using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OAuth.Application.Dtos;
using OAuth.Application.Interfaces;

namespace OAuth.Host.Controllers
{
    /// <summary>
    /// 微信用户
    /// </summary>
    [Route("api/[controller]")]
    public class SysWxUsersController : Controller
    {
        private readonly ISysWxUserService _service;

        public SysWxUsersController(ISysWxUserService personalService)
        {
            _service = personalService;
        }

        /// <summary>
        /// 获取微信用户基础信息
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="clientId">客户端id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<SysWxUserBaseDto> GetAsync([FromQuery] Guid userId, [FromQuery] string clientId)
        {
            return await _service.GetAsync(userId, clientId);
        }
    }
}
