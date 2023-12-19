﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Skalk.DAL.Migrations
{
    [DbContext(typeof(SkalkContext))]
    [Migration("20231214172220_Order")]
    partial class Order
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Skalk.DAL.Entities.ItemOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClickUrl")
                        .HasColumnType("text");

                    b.Property<string>("Mpn")
                        .HasColumnType("text");

                    b.Property<int>("OfferId")
                        .HasColumnType("integer");

                    b.Property<int>("OrderId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("ItemOrder");
                });

            modelBuilder.Entity("Skalk.DAL.Entities.ItemShoppingCart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Mpn")
                        .HasColumnType("text");

                    b.Property<int>("OfferId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<int>("ShoppingCartId")
                        .HasColumnType("integer");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("ShoppingCartId");

                    b.ToTable("ItemShoppingCarts");
                });

            modelBuilder.Entity("Skalk.DAL.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("CompanyName")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("OrderContractId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Skalk.DAL.Entities.OrderContract", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("Contract")
                        .HasColumnType("bytea");

                    b.Property<int>("OrderId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OrderId")
                        .IsUnique();

                    b.ToTable("OrderContract");
                });

            modelBuilder.Entity("Skalk.DAL.Entities.ShoppingCart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("ShoppingCarts");
                });

            modelBuilder.Entity("Skalk.DAL.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ContactInfo")
                        .HasColumnType("text");

                    b.Property<string>("Login")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<string>("Salt")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Skalk.DAL.Entities.ItemOrder", b =>
                {
                    b.HasOne("Skalk.DAL.Entities.Order", "Order")
                        .WithMany("ItemsOrder")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Skalk.DAL.Entities.ItemShoppingCart", b =>
                {
                    b.HasOne("Skalk.DAL.Entities.ShoppingCart", "ShoppingCart")
                        .WithMany("ItemShoppingCarts")
                        .HasForeignKey("ShoppingCartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ShoppingCart");
                });

            modelBuilder.Entity("Skalk.DAL.Entities.Order", b =>
                {
                    b.HasOne("Skalk.DAL.Entities.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Skalk.DAL.Entities.OrderContract", b =>
                {
                    b.HasOne("Skalk.DAL.Entities.Order", "Order")
                        .WithOne("OrderContract")
                        .HasForeignKey("Skalk.DAL.Entities.OrderContract", "OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Skalk.DAL.Entities.ShoppingCart", b =>
                {
                    b.HasOne("Skalk.DAL.Entities.User", "User")
                        .WithOne("ShoppingCart")
                        .HasForeignKey("Skalk.DAL.Entities.ShoppingCart", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Skalk.DAL.Entities.Order", b =>
                {
                    b.Navigation("ItemsOrder");

                    b.Navigation("OrderContract");
                });

            modelBuilder.Entity("Skalk.DAL.Entities.ShoppingCart", b =>
                {
                    b.Navigation("ItemShoppingCarts");
                });

            modelBuilder.Entity("Skalk.DAL.Entities.User", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("ShoppingCart");
                });
#pragma warning restore 612, 618
        }
    }
}