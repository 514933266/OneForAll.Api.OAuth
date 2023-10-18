using AutoMapper;
using OAuth.Application.Dtos;
using OAuth.Domain.AggregateRoots;
using OAuth.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth.Host.Profiles
{
    public class SysWebsiteApiSettingProfile : Profile
    {
        public SysWebsiteApiSettingProfile()
        {
            CreateMap<SysWebsiteApiSetting, SysWebsiteApiSettingDto>();
        }
    }
}
