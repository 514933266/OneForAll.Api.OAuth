using AutoMapper;
using OAuth.Domain.AggregateRoots;
using OAuth.Domain.Aggregates;
using OAuth.Domain.Models;
using OAuth.Public.Models;
using OneForAll.Core.OAuth;
using SysLog.HttpService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth.Host.Profiles
{
    public class SysUserProfile : Profile
    {
        public SysUserProfile()
        {
            CreateMap<SysUser, OAuthLogin>();
            CreateMap<SysLoginUserAggr, LoginUser>();

            CreateMap<WxmpLogin2SessionResponse, SysWxUser>();
            
        }
    }
}
