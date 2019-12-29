using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewKarma.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewKarma.Maps
{
    public class Car_CarModel_ProductMap : IEntityTypeConfiguration<RlCarModelProduct>
    {
        public void Configure(EntityTypeBuilder<RlCarModelProduct> builder)
        {
            builder.HasKey(a => new { a.CarId, a.ProductId });
            builder.HasOne(a => a.Car).WithMany(a => a.RlCarModelProduct).HasForeignKey(a => a.CarId);
            builder.HasOne(a => a.Product).WithMany(a => a.RlCarModelProduct).HasForeignKey(a =>a.ProductId);
        }
    }
}
