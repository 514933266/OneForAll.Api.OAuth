using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using OAuth.HttpService.Interfaces;
using OAuth.HttpService.Models;
using OAuth.Public.Models;
using OneForAll.Core.Extension;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OAuth.Host.Models;
using Microsoft.IdentityModel.Tokens;
using OneForAll.Core.OAuth;

namespace OAuth.Host.Filters
{
    /// <summary>
    /// Api日志
    /// </summary>
    public class ApiLogMiddleware
    {
        private readonly ISysApiLogHttpService _httpService;

        private Stopwatch _stopWatch;
        private readonly AuthConfig _authConfig;
        private readonly RequestDelegate _next;

        public ApiLogMiddleware(RequestDelegate request, AuthConfig authConfig, ISysApiLogHttpService httpService)
        {
            _next = request;
            _authConfig = authConfig;
            _httpService = httpService;
            _stopWatch = new Stopwatch();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _stopWatch.Restart();

            var request = context.Request;
            var descriptor = context.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>();

            if (descriptor == null)
                return;

            var loginUser = GetLoginUser(context);
            var userAgent = request.Headers["User-Agent"];
            var data = new SysApiLogRequest()
            {
                MoudleName = _authConfig.ClientName,
                MoudleCode = _authConfig.ClientCode,
                CreatorId = loginUser.Id,
                CreatorName = loginUser.Name,
                TenantId = loginUser.SysTenantId,
                Host = request.Host.ToString(),
                Url = request.Path.ToString(),
                Method = request.Method.ToUpper(),
                ContentType = request.ContentType ?? "application/json",
                UserAgent = userAgent.IsNullOrEmpty() ? "无" : userAgent.ToString(),
                IPAddress = request.HttpContext.Connection.RemoteIpAddress.ToString(),
                Action = descriptor.ActionName,
                Controller = descriptor.ControllerName
            };

            data.RequestBody = await GetRequestBody(context);
            data.ReponseBody = await GetResponseBody(context);

            // 响应完成记录时间和存入日志
            context.Response.OnCompleted(() =>
            {
                _stopWatch.Stop();

                data.TimeSpan = _stopWatch.Elapsed.ToString();
                data.StatusCode = context.Response.StatusCode.ToString();

                _httpService.AddAsync(data);

                return Task.CompletedTask;
            });
        }

        /// <summary>
        /// 获取请求内容
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task<string> GetRequestBody(HttpContext context)
        {
            var body = string.Empty;
            var method = context.Request.Method.ToUpper();
            var methods = new string[] { "POST", "PUT", "PATCH", "DELETED" };
            if (methods.Contains(method))
            {
                if (!(context.Request.ContentType.StartsWith("multipart/form-data") && context.Request.HasFormContentType))
                {
                    context.Request.EnableBuffering();

                    var result = await context.Request.BodyReader.ReadAsync();
                    var buffer = result.Buffer;

                    context.Request.Body.Seek(0, SeekOrigin.Begin);
                    body = Encoding.UTF8.GetString(buffer);
                }
            }
            else if (method.Equals("GET"))
            {
                body = context.Request.QueryString.Value;
            }
            return body;
        }

        /// <summary>
        /// 获取响应内容
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task<string> GetResponseBody(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;
                await _next(context);

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);

                return body;
            }
        }

        private LoginUser GetLoginUser(HttpContext context)
        {
            var role = context.User.Claims.FirstOrDefault(e => e.Type == UserClaimType.ROLE);
            var userId = context.User.Claims.FirstOrDefault(e => e.Type == UserClaimType.USER_ID);
            var name = context.User.Claims.FirstOrDefault(e => e.Type == UserClaimType.USER_NICKNAME);
            var tenantId = context.User.Claims.FirstOrDefault(e => e.Type == UserClaimType.TENANT_ID);

            return new LoginUser()
            {
                Id = userId == null ? Guid.Empty : new Guid(userId.Value),
                Name = name == null ? "无" : name?.Value,
                SysTenantId = tenantId == null ? Guid.Empty : new Guid(tenantId?.Value),
                IsDefault = role == null ? false : role.Value.Equals(UserRoleType.RULER)
            };
        }
    }
}