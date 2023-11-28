using Quartz;
using OAuth.Host.Models;
using System.Threading.Tasks;
using OAuth.Domain.Repositorys;
using OAuth.HttpService.Interfaces;
using System;
using IdentityServer4.Extensions;
using System.Linq;

namespace OAuth.Host.QuartzJobs
{
    /// <summary>
    /// 刷新微信access_token
    /// </summary>
    public class RefreshWxmpAccessTokenJob : IJob
    {
        private readonly AppInfo _config;
        private readonly IWxHttpService _wxHttpService;
        private readonly IScheduleJobHttpService _jobHttpService;
        private readonly ISysWxClientSettingRepository _wxSettingRepository;
        public RefreshWxmpAccessTokenJob(
            AppInfo config,
            IWxHttpService wxHttpService,
            IScheduleJobHttpService jobHttpService,
            ISysWxClientSettingRepository wxSettingRepository)
        {
            _config = config;
            _wxHttpService = wxHttpService;
            _jobHttpService = jobHttpService;
            _wxSettingRepository = wxSettingRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var num = 0;
            var clients = await _wxSettingRepository.GetListAsync();
            foreach (var client in clients)
            {
                var isInvalid = client.AccessTokenCreateTime == null ? true : client.AccessTokenCreateTime.Value.AddSeconds(client.AccessTokenExpiresIn) <= DateTime.Now;
                if (isInvalid)
                {
                    var response = await _wxHttpService.GetAccessTokenAsync(client.AppId, client.AppSecret);
                    if (!response.AccessToken.IsNullOrEmpty())
                    {
                        num++;
                        client.AccessToken = response.AccessToken;
                        client.AccessTokenExpiresIn = response.ExpiresIn;
                        client.AccessTokenCreateTime = DateTime.Now;
                    }
                }
            }
            num = await _wxSettingRepository.SaveChangesAsync();
            await _jobHttpService.LogAsync(_config.ClientCode, typeof(RefreshWxmpAccessTokenJob).Name, $"巡检刷新微信客户端AccessToken任务执行完成，共有{clients.Count()}个客户端,更新{num}个");
        }
    }
}
