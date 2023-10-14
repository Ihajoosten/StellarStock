﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using StellarStock.Infrastructure.Data;

#nullable disable

namespace StellarStock.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231014125346_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("StellarStock.Domain.Entities.InventoryItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("Category")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("character varying(75)");

                    b.Property<int>("PopularityScore")
                        .HasColumnType("integer");

                    b.Property<string>("SupplierId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("WarehouseId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("SupplierId");

                    b.HasIndex("WarehouseId");

                    b.ToTable("InventoryItems");
                });

            modelBuilder.Entity("StellarStock.Domain.Entities.Supplier", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ContactEmail")
                        .IsRequired()
                        .HasMaxLength(35)
                        .HasColumnType("character varying(35)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("character varying(25)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("character varying(25)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("StellarStock.Domain.Entities.Warehouse", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("character varying(25)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("character varying(25)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Warehouses");
                });

            modelBuilder.Entity("StellarStock.Domain.Entities.InventoryItem", b =>
                {
                    b.HasOne("StellarStock.Domain.Entities.Supplier", "Supplier")
                        .WithMany("SuppliedItems")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("StellarStock.Domain.Entities.Warehouse", "Warehouse")
                        .WithMany("StockedItems")
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.OwnsOne("StellarStock.Domain.ValueObjects.DateRangeVO", "ValidityPeriod", b1 =>
                        {
                            b1.Property<string>("InventoryItemId")
                                .HasColumnType("text");

                            b1.Property<DateTime>("EndDate")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("EndDate");

                            b1.Property<DateTime>("StartDate")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("StartDate");

                            b1.HasKey("InventoryItemId");

                            b1.ToTable("InventoryItems");

                            b1.WithOwner()
                                .HasForeignKey("InventoryItemId");
                        });

                    b.OwnsOne("StellarStock.Domain.ValueObjects.MoneyVO", "Money", b1 =>
                        {
                            b1.Property<string>("InventoryItemId")
                                .HasColumnType("text");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("numeric")
                                .HasColumnName("Amount");

                            b1.Property<string>("Currency")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("Currency");

                            b1.HasKey("InventoryItemId");

                            b1.ToTable("InventoryItems");

                            b1.WithOwner()
                                .HasForeignKey("InventoryItemId");
                        });

                    b.OwnsOne("StellarStock.Domain.ValueObjects.ProductCodeVO", "ProductCode", b1 =>
                        {
                            b1.Property<string>("InventoryItemId")
                                .HasColumnType("text");

                            b1.Property<string>("Code")
                                .IsRequired()
                                .HasMaxLength(25)
                                .HasColumnType("character varying(25)")
                                .HasColumnName("Code");

                            b1.HasKey("InventoryItemId");

                            b1.ToTable("InventoryItems");

                            b1.WithOwner()
                                .HasForeignKey("InventoryItemId");
                        });

                    b.OwnsOne("StellarStock.Domain.ValueObjects.QuantityVO", "Quantity", b1 =>
                        {
                            b1.Property<string>("InventoryItemId")
                                .HasColumnType("text");

                            b1.Property<int>("Value")
                                .HasColumnType("integer")
                                .HasColumnName("Value");

                            b1.HasKey("InventoryItemId");

                            b1.ToTable("InventoryItems");

                            b1.WithOwner()
                                .HasForeignKey("InventoryItemId");
                        });

                    b.Navigation("Money")
                        .IsRequired();

                    b.Navigation("ProductCode")
                        .IsRequired();

                    b.Navigation("Quantity")
                        .IsRequired();

                    b.Navigation("Supplier");

                    b.Navigation("ValidityPeriod")
                        .IsRequired();

                    b.Navigation("Warehouse");
                });

            modelBuilder.Entity("StellarStock.Domain.Entities.Supplier", b =>
                {
                    b.OwnsOne("StellarStock.Domain.ValueObjects.AddressVO", "Address", b1 =>
                        {
                            b1.Property<string>("SupplierId")
                                .HasColumnType("text");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(25)
                                .HasColumnType("character varying(25)")
                                .HasColumnName("Country");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
                                .HasMaxLength(10)
                                .HasColumnType("character varying(10)");

                            b1.Property<string>("Region")
                                .IsRequired()
                                .HasMaxLength(25)
                                .HasColumnType("character varying(25)");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("Street");

                            b1.HasKey("SupplierId");

                            b1.ToTable("Suppliers");

                            b1.WithOwner()
                                .HasForeignKey("SupplierId");
                        });

                    b.OwnsOne("StellarStock.Domain.ValueObjects.DateRangeVO", "ValidityPeriod", b1 =>
                        {
                            b1.Property<string>("SupplierId")
                                .HasColumnType("text");

                            b1.Property<DateTime>("EndDate")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("EndDate");

                            b1.Property<DateTime>("StartDate")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("StartDate");

                            b1.HasKey("SupplierId");

                            b1.ToTable("Suppliers");

                            b1.WithOwner()
                                .HasForeignKey("SupplierId");
                        });

                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("ValidityPeriod")
                        .IsRequired();
                });

            modelBuilder.Entity("StellarStock.Domain.Entities.Warehouse", b =>
                {
                    b.OwnsOne("StellarStock.Domain.ValueObjects.AddressVO", "Address", b1 =>
                        {
                            b1.Property<string>("WarehouseId")
                                .HasColumnType("text");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(25)
                                .HasColumnType("character varying(25)")
                                .HasColumnName("Country");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
                                .HasMaxLength(10)
                                .HasColumnType("character varying(10)");

                            b1.Property<string>("Region")
                                .IsRequired()
                                .HasMaxLength(25)
                                .HasColumnType("character varying(25)");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("Street");

                            b1.HasKey("WarehouseId");

                            b1.ToTable("Warehouses");

                            b1.WithOwner()
                                .HasForeignKey("WarehouseId");
                        });

                    b.Navigation("Address")
                        .IsRequired();
                });

            modelBuilder.Entity("StellarStock.Domain.Entities.Supplier", b =>
                {
                    b.Navigation("SuppliedItems");
                });

            modelBuilder.Entity("StellarStock.Domain.Entities.Warehouse", b =>
                {
                    b.Navigation("StockedItems");
                });
#pragma warning restore 612, 618
        }
    }
}