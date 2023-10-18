using System;
using OAuth.Domain;
using OAuth.Domain.AggregateRoots;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OAuth.Host
{
    public partial class OneForAll_BaseContext : DbContext
    {

        public OneForAll_BaseContext(DbContextOptions<OneForAll_BaseContext> options)
            : base(options)
        {

        }

        public virtual DbSet<SysTenant> SysTenant { get; set; }
        public virtual DbSet<SysUser> SysUser { get; set; }
        public virtual DbSet<SysWechatUser> SysWxLoginUser { get; set; }
        public virtual DbSet<SysWxClientSetting> SysWxClientSetting { get; set; }
        public virtual DbSet<SysWebsiteSetting> SysWebsiteSetting { get; set; }
        public virtual DbSet<SysWebsiteApiSetting> SysWebsiteApiSetting { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SysTenant>(entity =>
            {
                entity.ToTable("Sys_Tenant");
            });
            modelBuilder.Entity<SysUser>(entity =>
            {
                entity.ToTable("Sys_User");
            });
            modelBuilder.Entity<SysWechatUser>(entity =>
            {
                entity.ToTable("Sys_WechatUser");
            });
            modelBuilder.Entity<SysWxClientSetting>(entity =>
            {
                entity.ToTable("Sys_WxClientSetting");
            });
            modelBuilder.Entity<SysWebsiteSetting>(entity =>
            {
                entity.ToTable("Sys_WebsiteSetting");
            });
            modelBuilder.Entity<SysWebsiteApiSetting>(entity =>
            {
                entity.ToTable("Sys_WebsiteApiSetting");
            });
        }
    }
}
