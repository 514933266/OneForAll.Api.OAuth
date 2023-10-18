using Microsoft.EntityFrameworkCore;
using OAuth.Domain.AggregateRoots;
using OAuth.Domain.Repositorys;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Repository
{
    /// <summary>
    /// 微信登录用户
    /// </summary>
    public class SysWechatUserRepository : Repository<SysWechatUser>, ISysWechatUserRepository
    {
        public SysWechatUserRepository(DbContext context)
            : base(context)
        {

        }
    }
}
