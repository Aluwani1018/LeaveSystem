using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveSystem.Data.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace LeaveSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<Employee, Role, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Leave> Leave { get; set; }
        public DbSet<LeaveStatus> LeaveStatus { get; set; }
        public DbSet<PublicHoliday> PublicHoliday { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Employee>().HasMany(u => u.Claims).WithOne().HasForeignKey(c => c.UserId).IsRequired();
            builder.Entity<Employee>().HasMany(u => u.Roles).WithOne().HasForeignKey(r => r.UserId).IsRequired();
            builder.Entity<Employee>()
                .HasMany(e => e.Employees)
                .WithOne()
                .HasForeignKey(m => m.Id);

            builder.Entity<Employee>()
                .HasOne(c => c.Manager)
                .WithMany()
                .HasForeignKey(r => r.ManagerId);
            builder.Entity<Employee>()
               .HasMany(c => c.Leaves)
               .WithOne()
               .HasForeignKey(r => r.EmployeeId);

            builder.Entity<Employee>()
                .Property(x => x.EmployeeNumber)
                .IsRequired();
                

            builder.Entity<Role>().HasMany(r => r.Claims).WithOne().HasForeignKey(c => c.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Role>().HasMany(r => r.Users).WithOne().HasForeignKey(r => r.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Employee>().ToTable("Employee");

            builder.Entity<Role>().ToTable("Role");

            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim");

            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRole");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserToken");

            builder.Entity<Leave>()
                .HasOne(c => c.LeaveStatus)
                .WithMany()
                .HasForeignKey(x => x.StatusId)
                .IsRequired();

           

            builder.Entity<Leave>()
               .Property(c => c.FromDate)
               .IsRequired();

            builder.Entity<Leave>()
                .Property(c => c.ToDate)
                .IsRequired();

            builder.Entity<Leave>()
                .Property(c => c.Reason)
                .HasMaxLength(50);

            builder.Entity<Leave>()
                .ToTable("Leave");

            builder.Entity<LeaveStatus>()
                .Property(c => c.Name)
                .IsRequired();

            builder.Entity<LeaveStatus>()
                .ToTable("LeaveStatus");

            builder.Entity<PublicHoliday>()
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Entity<PublicHoliday>()
                .Property(p => p.Date)
                .IsRequired();

            builder.Entity<PublicHoliday>()
                .ToTable("PublicHoliday");

        }

    }
}
