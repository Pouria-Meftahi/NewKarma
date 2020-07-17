using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewKarma.Areas.Identity.Data;
using NewKarma.Models.Domain;

namespace NewKarma.Models
{
    public partial class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //   optionsBuilder.UseSqlServer("Server=.;Database=NewKarma;Trusted_Connection=True;");
        //    //optionsBuilder.UseSqlServer("Server=.\\MSSQLSERVER2016;Database=KarmaDB;User ID=KarmayadaDB;Password=#vw8T9o2;");
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);//Hack:Whats Hapening There
            modelBuilder.ApplyConfiguration(new Car_CarModel_ProductMap());
            modelBuilder.Entity<Product>().Property(a => a.CreatedDate).HasDefaultValueSql("CONVERT(datetime,GetDate())");
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<RlCarModelProduct> RlCarModelProducts { get; set; }
    }
}
