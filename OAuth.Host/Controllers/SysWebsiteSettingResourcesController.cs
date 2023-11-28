using System;
using System.Threading.Tasks;
using OAuth.Application.Dtos;
using OAuth.Application.Interfaces;
using OAuth.Domain.Models;
using OAuth.Host.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneForAll.Core;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using OneForAll.Core.Upload;

namespace OAuth.Host.Controllers
{
    /// <summary>
    /// 网站设置
    /// </summary>
    [Route("api/[controller]")]
    public class SysWebsiteSettingResourcesController : Controller
    {
        private readonly ISysWebsiteSettingService _service;
        public SysWebsiteSettingResourcesController(ISysWebsiteSettingService service)
        {
            _service = service;
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <returns>实体</returns>
        [HttpGet]
        [Route("Current")]
        public async Task<SysWebsiteSettingDto> GetAsync()
        {
            var origin = Request.Headers["Origin"].ToString();
            return await _service.GetAsync(origin);
        }
    }
}
