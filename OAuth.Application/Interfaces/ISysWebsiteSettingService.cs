using OAuth.Application.Dtos;
using OAuth.Domain.Models;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OneForAll.Core.Upload;
using System.IO;

namespace OAuth.Application.Interfaces
{
    /// <summary>
    /// 应用权限
    /// </summary>
    public interface ISysWebsiteSettingService
    {
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="host">域名</param>
        /// <returns>实体</returns>
        Task<SysWebsiteSettingDto> GetAsync(string host);
    }
}
