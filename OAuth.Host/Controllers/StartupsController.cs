using Microsoft.AspNetCore.Mvc;
using OneForAll.Core;
using Quartz;
using System.Reflection;
using System.Threading.Tasks;

namespace OAuth.Host.Controllers
{
    [Route("api/[controller]")]
    public class StartupsController : Controller
    {

        private readonly string _assemblyName;
        private readonly ISchedulerFactory _schedulerFactory;

        public StartupsController(ISchedulerFactory schedulerFactory)
        {
            _assemblyName = Assembly.GetEntryAssembly().GetName().Name;
            _schedulerFactory = schedulerFactory;
        }

        [HttpGet]
        public string Get()
        {
            return "项目启动成功...";
        }

        /// <summary>
        /// 暂停定时任务
        /// </summary>
        /// <param name="jobName">定时任务名称</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Default/Jobs/{jobName}/Stop")]
        public async Task<BaseMessage> PauseJob(string jobName)
        {
            var msg = new BaseMessage();
            var scheduler = await _schedulerFactory.GetScheduler();
            var jobKey = new JobKey($"{_assemblyName}.QuartzJobs.{jobName}");
            var job = scheduler.GetJobDetail(jobKey);
            if (job.Result == null)
                return msg.Fail("不存在该定时任务");
            await scheduler.PauseJob(jobKey);
            return msg.Success("暂停成功");
        }

        /// <summary>
        /// 重启定时任务
        /// </summary>
        /// <param name="jobName">定时任务名称</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Default/Jobs/{jobName}/Resume")]
        public async Task<BaseMessage> ResumeJob(string jobName)
        {
            var msg = new BaseMessage();
            var scheduler = await _schedulerFactory.GetScheduler();
            var jobKey = new JobKey($"{_assemblyName}.QuartzJobs.{jobName}");
            var job = scheduler.GetJobDetail(jobKey);
            if (job.Result == null)
                return msg.Fail("不存在该定时任务");
            await scheduler.PauseJob(jobKey);
            return msg.Success("启动成功");
        }

        /// <summary>
        /// 执行一次定时任务
        /// </summary>
        /// <param name="jobName">定时任务名称</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Default/Jobs/{jobName}/Excute")]
        public async Task<BaseMessage> ExcuteJob(string jobName)
        {
            var msg = new BaseMessage();
            var scheduler = await _schedulerFactory.GetScheduler();
            var jobKey = new JobKey($"{_assemblyName}.QuartzJobs.{jobName}");
            var job = scheduler.GetJobDetail(jobKey);
            if (job.Result == null)
                return msg.Fail("不存在该定时任务");
            await scheduler.TriggerJob(jobKey);
            return msg.Success("任务触发执行成功");
        }
    }
}