using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewKarma.Areas.Identity.Data;
using NewKarma.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewKarma.Models
{
    public partial class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>, ApplicationRoleClaim, IdentityUserToken<string>>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=NewKarma;Trusted_Connection=True;");
            //optionsBuilder.UseSqlServer("Server=.\\MSSQLSERVER2016;Database=KarmaDB;User ID=KarmayadaDB;Password=#vw8T9o2;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);//Hack:Whats Hapening There
            modelBuilder.ApplyConfiguration(new Maps.Car_CarModel_ProductMap());
            modelBuilder.Entity<ApplicationRole>().ToTable("AppRole");
            modelBuilder.Entity<ApplicationUserRole>().ToTable("AppUserRole");
            modelBuilder.Entity<ApplicationUser>().ToTable("AppUser");
            modelBuilder.Entity<ApplicationRoleClaim>().ToTable("AppRoleClaim");

            modelBuilder.Entity<ApplicationUserRole>()
                .HasOne(userRole => userRole.Role)
                .WithMany(role => role.Users)
                .HasForeignKey(f => f.RoleId);
            modelBuilder.Entity<ApplicationUserRole>()
                .HasOne(userRole => userRole.User)
                .WithMany(role => role.Roles)
                .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<ApplicationRoleClaim>()
                .HasOne(roleClaim => roleClaim.Role)
                .WithMany(claim => claim.Claims)
                .HasForeignKey(f => f.RoleId);
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<RlCarModelProduct> RlCarModelProducts{ get; set; }
    }
}
