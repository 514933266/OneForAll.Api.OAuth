using System;
using OAuth.Domain;
using OAuth.Domain.AggregateRoots;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OAuth.Host
{
    public partial class SysDbContext : DbContext
    {

        public SysDbContext(DbContextOptions<SysDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<SysTenant> SysTenant { get; set; }
        public virtual DbSet<SysUser> SysUser { get; set; }
        public virtual DbSet<SysUserRiskRecord> SysUserRiskRecord { get; set; }
        public virtual DbSet<SysWxUser> SysWxLoginUser { get; set; }
        public virtual DbSet<SysClient> SysClient { get; set; }
        public virtual DbSet<SysWxClient> SysWxClient { get; set; }
        public virtual DbSet<SysWxClientContact> SysWxClientContact { get; set; }
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
            modelBuilder.Entity<SysUserRiskRecord>(entity =>
            {
                entity.ToTable("Sys_UserRiskRecord");
            });
            modelBuilder.Entity<SysWxUser>(entity =>
            {
                entity.ToTable("Sys_WechatUser");
            });
            modelBuilder.Entity<SysClient>(entity =>
            {
                entity.ToTable("Sys_Client");
            });
            modelBuilder.Entity<SysWxClient>(entity =>
            {
                entity.ToTable("Sys_WxClient");
            });
            modelBuilder.Entity<SysWxClientContact>(entity =>
            {
                entity.ToTable("Sys_WxClientContact");
            });
            modelBuilder.Entity<SysWxClientContact>(entity =>
            {
                entity.ToTable("Sys_WxClientContact");
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
