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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SysTenant>(entity =>
            {
                entity.ToTable("Sys_Tenant");
            });

            modelBuilder.Entity<SysUser>(entity =>
            {
                entity.ToTable("Sys_User");

                entity.HasOne(e => e.SysTenant)
                    .WithMany(e => e.SysUsers)
                    .HasForeignKey(e => e.SysTenantId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
