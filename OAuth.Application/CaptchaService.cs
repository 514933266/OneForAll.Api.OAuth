using AutoMapper;
using Microsoft.Extensions.Configuration;
using OAuth.Application.Interfaces;
using OAuth.Domain.Interfaces;
using OAuth.Domain.Repositorys;
using SysLog.HttpService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Application
{
    /// <summary>
    /// 验证码
    /// </summary>
    public class CaptchaService : ICaptchaService
    {
        private readonly ICaptchaManager _manager;
        public CaptchaService(ICaptchaManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// 获取Base64验证码
        /// </summary>
        /// <param name="userName">登录账号</param>
        /// <returns>结果</returns>
        public async Task<string> GetImageBase64Async(string userName)
        {
            return await _manager.GetStrCodeBase64Async(userName);
        }
    }
}
