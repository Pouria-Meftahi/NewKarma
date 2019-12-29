﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NewKarma.Models;

namespace NewKarma.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20190717070818_migInit")]
    partial class migInit
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NewKarma.Models.Domain.Category", b =>
                {
                    b.Property<int>("catId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Icon")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("catId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("NewKarma.Models.Domain.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CategorycatId");

                    b.Property<string>("Desc")
                        .IsRequired();

                    b.Property<string>("Img")
                        .IsRequired();

                    b.Property<int>("Price");

                    b.Property<bool>("Situation");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.HasKey("ProductId");

                    b.HasIndex("CategorycatId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("NewKarma.Models.Domain.Product", b =>
                {
                    b.HasOne("NewKarma.Models.Domain.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategorycatId");
                });
#pragma warning restore 612, 618
        }
    }
}
