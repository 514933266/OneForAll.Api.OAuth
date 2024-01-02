using Microsoft.AspNetCore.Http;
using OAuth.Host.Models;
using OAuth.HttpService.Interfaces;
using OAuth.HttpService.Models;
using System;
using System.Threading.Tasks;

namespace OAuth.Host.Filters
{
    /// <summary>
    /// 全局异常
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AuthConfig _authConfig;
        private readonly ISysGlobalExceptionLogHttpService _httpService;

        public GlobalExceptionMiddleware(RequestDelegate next, AuthConfig authConfig, ISysGlobalExceptionLogHttpService httpService)
        {
            _next = next;
            _authConfig = authConfig;
            _httpService = httpService;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                var features = context.Features;
            }
            catch (Exception e)
            {
                await HandleException(context, e);
            }
        }

        private async Task HandleException(HttpContext context, Exception e)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/json;charset=utf-8;";

            #region 记录日志
            await _httpService.AddAsync(new SysGlobalExceptionLogRequest()
            {
                MoudleName = _authConfig.ClientName,
                MoudleCode = _authConfig.ClientCode,
                Name = e.Message,
                Content = e.StackTrace
            });
            #endregion

            await context.Response.WriteAsync($"抱歉，服务器发生异常:{e.Message}");
        }
    }
}
