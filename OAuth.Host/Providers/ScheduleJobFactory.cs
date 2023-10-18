using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;

namespace OAuth.Host.Providers
{
    /// <summary>
    /// 任务调度工厂
    /// </summary>
    public class ScheduleJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public ScheduleJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 实例化定时任务
        /// </summary>
        /// <param name="bundle"></param>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return _serviceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
        }

        /// <summary>
        /// 销毁定时任务
        /// </summary>
        /// <param name="job"></param>
        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}
