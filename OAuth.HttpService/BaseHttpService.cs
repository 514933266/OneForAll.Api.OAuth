using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using OneForAll.Core.Extension;
using OAuth.Public.Models;

namespace OAuth.HttpService
{
    /// <summary>
    /// Http基类
    /// </summary>
    public class BaseHttpService
    {
        private readonly string AUTH_KEY = "Authorization";
        protected readonly IHttpContextAccessor _httpContext;
        protected readonly IHttpClientFactory _httpClientFactory;
        public BaseHttpService(
            IHttpContextAccessor httpContext,
            IHttpClientFactory httpClientFactory)
        {
            _httpContext = httpContext;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// 登录token
        /// </summary>
        protected string Token
        {
            get
            {
                var context = _httpContext.HttpContext;
                if (context != null)
                {
                    return context.Request.Headers
                      .FirstOrDefault(w => w.Key.Equals(AUTH_KEY))
                      .Value.TryString();
                }
                return "";
            }
        }

        protected Guid UserId
        {
            get
            {
                var userId = _httpContext.HttpContext
                .User
                .Claims
                .FirstOrDefault(e => e.Type == UserClaimType.USER_ID);

                if (userId != null)
                {
                    return new Guid(userId.Value);
                }
                return Guid.Empty;
            }
        }

        protected string UserName
        {
            get
            {
                var username = _httpContext.HttpContext
                .User
                .Claims
                .FirstOrDefault(e => e.Type == UserClaimType.USERNAME);

                if (username != null)
                {
                    return username.Value;
                }
                return null;
            }
        }

        protected Guid TenantId
        {
            get
            {
                var tenantId = _httpContext.HttpContext
                .User
                .Claims
                .FirstOrDefault(e => e.Type == UserClaimType.TENANT_ID);

                if (tenantId != null)
                {
                    return new Guid(tenantId.Value);
                }
                return Guid.Empty;
            }
        }

        protected LoginUser LoginUser
        {
            get
            {
                var name = _httpContext.HttpContext
                .User
                .Claims
                .FirstOrDefault(e => e.Type == UserClaimType.USER_NICKNAME);

                var role = _httpContext.HttpContext
                .User
                .Claims
                .FirstOrDefault(e => e.Type == UserClaimType.ROLE);

                return new LoginUser()
                {
                    Id = UserId,
                    Name = name?.Value,
                    UserName = UserName,
                    TenantId = TenantId
                };
            }
        }

        /// <summary>
        /// 获取HttpClient
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected HttpClient GetHttpClient(string name)
        {
            var client = _httpClientFactory.CreateClient(name);
            if (!Token.IsNullOrEmpty())
            {
                client.DefaultRequestHeaders.Add(AUTH_KEY, Token);
            }
            return client;
        }
    }
}
