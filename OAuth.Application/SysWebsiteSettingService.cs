using AutoMapper;
using OAuth.Application.Dtos;
using OAuth.Application.Interfaces;
using System.Threading.Tasks;
using OneForAll.Core.Extension;
using OAuth.Domain.Repositorys;
using OAuth.Domain.Aggregates;

namespace OAuth.Application
{
    /// <summary>
    /// 应用权限
    /// </summary>
    public class SysWebsiteSettingService : ISysWebsiteSettingService
    {
        private readonly IMapper _mapper;
        private readonly ISysWebsiteSettingRepository _repository;

        public SysWebsiteSettingService(
            IMapper mapper,
            ISysWebsiteSettingRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="host">域名</param>
        /// <returns>实体</returns>
        public async Task<SysWebsiteSettingDto> GetAsync(string host)
        {
            var result = new SysWebsiteSettingDto() { Name = "未注册授权域名" };
            var data = await _repository.GetByHostWithContactAsync(host);
            if (data == null)
                return result;
            return _mapper.Map<SysWebsiteSettingAggr, SysWebsiteSettingDto>(data); ;
        }
    }
}
