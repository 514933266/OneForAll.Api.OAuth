using OAuth.Domain.Repositorys;
using OAuth.Host.Models;
using OAuth.HttpService.Interfaces;
using OAuth.HttpService.Models;
using OneForAll.Core.Extension;
using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth.Host.QuartzJobs
{
    /// <summary>
    /// 同步用户手机号
    /// </summary>
    public class SynUserMobileJob : IJob
    {
        private readonly AuthConfig _config;
        private readonly ISysUserRepository _userRepository;
        private readonly IScheduleJobHttpService _jobHttpService;
        private readonly ISysGlobalExceptionLogHttpService _logHttpService;
        public SynUserMobileJob(
            AuthConfig config,
            ISysUserRepository userRepository,
            IScheduleJobHttpService jobHttpService,
            ISysGlobalExceptionLogHttpService logHttpService)
        {
            _config = config;
            _userRepository = userRepository;
            _jobHttpService = jobHttpService;
            _logHttpService = logHttpService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var num = 0;
                var users = await _userRepository.GetListAsync(w => w.Mobile == "");
                foreach (var user in users)
                {
                    if (user.UserName.IsMobile())
                        user.Mobile = user.UserName;
                }
                num = await _userRepository.SaveChangesAsync();
                await _jobHttpService.LogAsync(_config.ClientCode, typeof(SynUserMobileJob).Name, $"同步用户手机号任务执行完成，共有{users.Count()}个用户,更新{num}个");
            }
            catch (Exception ex)
            {
                await _logHttpService.AddAsync(new SysGlobalExceptionLogRequest
                {
                    MoudleName = _config.ClientName,
                    MoudleCode = _config.ClientCode,
                    Name = ex.Message,
                    Content = ex.InnerException == null ? ex.StackTrace : ex.InnerException.StackTrace
                });
            }
        }
    }
}
