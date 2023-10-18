using AutoMapper;
using OAuth.Application.Dtos;
using OAuth.Domain.AggregateRoots;
using OAuth.Domain.Aggregates;
using OAuth.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth.Host.Profiles
{
    public class SysWebsiteSettingProfile : Profile
    {
        public SysWebsiteSettingProfile()
        {
            CreateMap<SysWebsiteSetting, SysWebsiteSettingDto>();
            CreateMap<SysWebsiteSettingAggr, SysWebsiteSettingDto>()
                .ForMember(t => t.Apis, a => a.MapFrom(s => s.SysWebsiteApiSettings));
        }
    }
}
