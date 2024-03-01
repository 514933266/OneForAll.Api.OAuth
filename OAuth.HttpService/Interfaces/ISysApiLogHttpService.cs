using OAuth.HttpService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.HttpService.Interfaces
{
    /// <summary>
    /// Api日志
    /// </summary>
    public interface ISysApiLogHttpService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns></returns>
        Task AddAsync(SysApiLogRequest form);
    }
}

