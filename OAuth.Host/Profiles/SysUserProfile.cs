using AutoMapper;
using OAuth.Domain.AggregateRoots;
using OAuth.Domain.Models;
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
            CreateMap<SysUser, OAuthLoginUser>()
                .ForMember(t => t.TenantId, a => a.MapFrom(e => e.SysTenantId))
                .ForMember(t => t.IsDefaultTenant, a => a.MapFrom(e => e.SysTenant.IsDefault));
        }
    }
}
