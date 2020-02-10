using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewKarma.Models.Domain
{
    public class RlCarModelProduct
    {
        public int CarId { get; set; }
        public int ProductId { get; set; }
        public Car Car { get; set; }
        public Product Product { get; set; }
    }
    public class Car_CarModel_ProductMap : IEntityTypeConfiguration<RlCarModelProduct>
    {
        public void Configure(EntityTypeBuilder<RlCarModelProduct> builder)
        {
            builder.HasKey(a => new { a.CarId, a.ProductId });
            builder.HasOne(a => a.Car).WithMany(a => a.RlCarModelProduct).HasForeignKey(a => a.CarId);
            builder.HasOne(a => a.Product).WithMany(a => a.RlCarModelProduct).HasForeignKey(a => a.ProductId);
        }
    }
}
