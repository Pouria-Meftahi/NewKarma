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
    [Migration("20190825145724_mig11RemoveCarsRel")]
    partial class mig11RemoveCarsRel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("NewKarma.Areas.Identity.Data.ApplicationRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AppRole");
                });

            modelBuilder.Entity("NewKarma.Areas.Identity.Data.ApplicationRoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AppRoleClaim");
                });

            modelBuilder.Entity("NewKarma.Areas.Identity.Data.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<DateTime>("RegisterDate");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("Status");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AppUser");
                });

            modelBuilder.Entity("NewKarma.Areas.Identity.Data.ApplicationUserRole", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AppUserRole");
                });

            modelBuilder.Entity("NewKarma.Models.Domain.Brand", b =>
                {
                    b.Property<int>("BrandId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<string>("UserIDFK");

                    b.HasKey("BrandId");

                    b.HasIndex("UserIDFK");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("NewKarma.Models.Domain.Category", b =>
                {
                    b.Property<int>("CatId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Icon")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<string>("UserIDFK");

                    b.HasKey("CatId");

                    b.HasIndex("UserIDFK");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("NewKarma.Models.Domain.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BrandIDFK");

                    b.Property<int>("CatIDFK");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Img")
                        .IsRequired();

                    b.Property<bool>("Situation");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.Property<string>("UserIDFK");

                    b.HasKey("ProductId");

                    b.HasIndex("BrandIDFK");

                    b.HasIndex("CatIDFK");

                    b.HasIndex("UserIDFK");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("NewKarma.Areas.Identity.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("NewKarma.Areas.Identity.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("NewKarma.Areas.Identity.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NewKarma.Areas.Identity.Data.ApplicationRoleClaim", b =>
                {
                    b.HasOne("NewKarma.Areas.Identity.Data.ApplicationRole", "Role")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NewKarma.Areas.Identity.Data.ApplicationUserRole", b =>
                {
                    b.HasOne("NewKarma.Areas.Identity.Data.ApplicationRole", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NewKarma.Areas.Identity.Data.ApplicationUser", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NewKarma.Models.Domain.Brand", b =>
                {
                    b.HasOne("NewKarma.Areas.Identity.Data.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserIDFK");
                });

            modelBuilder.Entity("NewKarma.Models.Domain.Category", b =>
                {
                    b.HasOne("NewKarma.Areas.Identity.Data.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserIDFK");
                });

            modelBuilder.Entity("NewKarma.Models.Domain.Product", b =>
                {
                    b.HasOne("NewKarma.Models.Domain.Brand", "Brand")
                        .WithMany("Products")
                        .HasForeignKey("BrandIDFK")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NewKarma.Models.Domain.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CatIDFK")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NewKarma.Areas.Identity.Data.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserIDFK");
                });
#pragma warning restore 612, 618
        }
    }
}
