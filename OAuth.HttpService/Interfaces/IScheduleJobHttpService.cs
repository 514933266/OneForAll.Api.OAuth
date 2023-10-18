using OAuth.HttpService.Models;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.HttpService.Interfaces
{
    /// <summary>
    /// 定时任务调度中心
    /// </summary>
    public interface IScheduleJobHttpService
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="request">实体</param>
        /// <returns></returns>
        Task<BaseErrType> RegisterAsync(JobRegisterRequest request);

        /// <summary>
        /// 定时服务下线
        /// </summary>
        /// <param name="appId">应用程序id</param>
        /// <param name="taskName">定时任务名称</param>
        /// <returns结果</returns>
        Task DownLineAsync(string appId, string taskName);

        /// <summary>
        /// 定时服务运行日志
        /// </summary>
        /// <param name="appId">应用程序id</param>
        /// <param name="taskName">定时任务名称</param>
        /// <param name="log">日志内容</param>
        /// <returns结果</returns>
        Task<BaseErrType> LogAsync(string appId, string taskName, string log);
    }
}
