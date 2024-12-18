﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebAppNorthwind.Database;

#nullable disable

namespace WebAppNorthwind.Migrations
{
    [DbContext(typeof(NWContext))]
    partial class NWContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebAppNorthwind.Database.Customers", b =>
                {
                    b.Property<string>("CustomerID")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("CustomerID");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Address");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("City");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("CompanyName");

                    b.Property<string>("ContactName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("ContactName");

                    b.Property<string>("ContactTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("ContactTitle");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Country");

                    b.Property<string>("Fax")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Fax");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Phone");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("PostalCode");

                    b.Property<string>("Region")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Region");

                    b.HasKey("CustomerID");

                    b.ToTable("Customer", "dbo");
                });
#pragma warning restore 612, 618
        }
    }
}