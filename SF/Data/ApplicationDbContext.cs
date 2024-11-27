using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SF.Models;
using System.Reflection.Emit;

namespace SF.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<RoleGroup> RoleGroups { get; set; }
        public DbSet<RoleGroupRoles> RoleGroupRoles { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RoleGroupRoles>()
                .HasKey(r => new { r.RoleGroupId, r.RoleName });

            
            builder.Entity<RoleGroupRoles>()
                .HasOne(rgr => rgr.RoleGroup)
                .WithMany(rg => rg.RoleGroupRoles)
                .HasForeignKey(rgr => rgr.RoleGroupId);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Company)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.CompanyId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
