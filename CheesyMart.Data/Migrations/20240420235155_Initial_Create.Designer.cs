﻿// <auto-generated />
using System;
using CheesyMart.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CheesyMart.Data.Migrations
{
    [DbContext(typeof(MainDbContext))]
    [Migration("20240420235155_Initial_Create")]
    partial class Initial_Create
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CheesyMart.Data.Entities.CheeseProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CheeseType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<decimal>("PricePerKilo")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("CheeseProducts");
                });

            modelBuilder.Entity("CheesyMart.Data.Entities.ProductImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CheeseProductId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("CheeseProductId");

                    b.ToTable("ProductImages");
                });

            modelBuilder.Entity("CheesyMart.Data.Entities.ProductImageData", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("AlternateText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("MimeType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ProductImages", (string)null);
                });

            modelBuilder.Entity("CheesyMart.Data.Entities.ProductImage", b =>
                {
                    b.HasOne("CheesyMart.Data.Entities.CheeseProduct", "CheeseProduct")
                        .WithMany("Images")
                        .HasForeignKey("CheeseProductId");

                    b.Navigation("CheeseProduct");
                });

            modelBuilder.Entity("CheesyMart.Data.Entities.ProductImageData", b =>
                {
                    b.HasOne("CheesyMart.Data.Entities.ProductImage", null)
                        .WithOne("ProductImageData")
                        .HasForeignKey("CheesyMart.Data.Entities.ProductImageData", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CheesyMart.Data.Entities.CheeseProduct", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("CheesyMart.Data.Entities.ProductImage", b =>
                {
                    b.Navigation("ProductImageData")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
