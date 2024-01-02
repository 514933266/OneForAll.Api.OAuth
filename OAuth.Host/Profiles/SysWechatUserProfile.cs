using AutoMapper;
using OAuth.Application.Dtos;
using OAuth.Domain.AggregateRoots;
using OAuth.Domain.Aggregates;
using OAuth.Domain.Models;
using SysLog.HttpService.Models;

namespace OAuth.Host.Profiles
{
    public class SysWechatUserProfile : Profile
    {
        public SysWechatUserProfile()
        {
            CreateMap<SysWxUser, SysWxUserBaseDto>()
                .ForMember(t => t.UserId, a => a.MapFrom(e => e.SysUserId));
        }
    }
}