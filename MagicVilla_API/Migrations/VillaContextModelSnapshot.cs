﻿// <auto-generated />
using System;
using MagicVilla_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MagicVilla_API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class VillaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MagicVilla_API.Models.Villa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Amenity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Detail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Fee")
                        .HasColumnType("real");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Occupants")
                        .HasColumnType("int");

                    b.Property<float>("SquareMeters")
                        .HasColumnType("real");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Villas");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amenity = "Amenity 1",
                            CreationDate = new DateTime(2023, 4, 4, 17, 40, 44, 404, DateTimeKind.Local).AddTicks(9117),
                            Detail = "Villa detail 1",
                            Fee = 1f,
                            ImageUrl = "",
                            Name = "Villa 1",
                            Occupants = 10,
                            SquareMeters = 80.5f,
                            UpdateDate = new DateTime(2023, 4, 4, 17, 40, 44, 404, DateTimeKind.Local).AddTicks(9131)
                        },
                        new
                        {
                            Id = 2,
                            Amenity = "Amenity 2",
                            CreationDate = new DateTime(2023, 4, 4, 17, 40, 44, 404, DateTimeKind.Local).AddTicks(9135),
                            Detail = "Villa detail 2",
                            Fee = 2f,
                            ImageUrl = "",
                            Name = "Villa 2",
                            Occupants = 20,
                            SquareMeters = 200f,
                            UpdateDate = new DateTime(2023, 4, 4, 17, 40, 44, 404, DateTimeKind.Local).AddTicks(9136)
                        },
                        new
                        {
                            Id = 3,
                            Amenity = "Amenity 3",
                            CreationDate = new DateTime(2023, 4, 4, 17, 40, 44, 404, DateTimeKind.Local).AddTicks(9139),
                            Detail = "Villa detail 3",
                            Fee = 3f,
                            ImageUrl = "",
                            Name = "Villa 3",
                            Occupants = 30,
                            SquareMeters = 300f,
                            UpdateDate = new DateTime(2023, 4, 4, 17, 40, 44, 404, DateTimeKind.Local).AddTicks(9140)
                        },
                        new
                        {
                            Id = 4,
                            Amenity = "Amenity 4",
                            CreationDate = new DateTime(2023, 4, 4, 17, 40, 44, 404, DateTimeKind.Local).AddTicks(9142),
                            Detail = "Villa detail 4",
                            Fee = 4f,
                            ImageUrl = "",
                            Name = "Villa 4",
                            Occupants = 40,
                            SquareMeters = 400f,
                            UpdateDate = new DateTime(2023, 4, 4, 17, 40, 44, 404, DateTimeKind.Local).AddTicks(9143)
                        },
                        new
                        {
                            Id = 5,
                            Amenity = "Amenity 5",
                            CreationDate = new DateTime(2023, 4, 4, 17, 40, 44, 404, DateTimeKind.Local).AddTicks(9146),
                            Detail = "Villa detail 5",
                            Fee = 5f,
                            ImageUrl = "",
                            Name = "Villa 5",
                            Occupants = 50,
                            SquareMeters = 500f,
                            UpdateDate = new DateTime(2023, 4, 4, 17, 40, 44, 404, DateTimeKind.Local).AddTicks(9147)
                        });
                });

            modelBuilder.Entity("MagicVilla_API.Models.VillaNumber", b =>
                {
                    b.Property<int>("VillaNro")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descripcion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("VillaId")
                        .HasColumnType("int");

                    b.HasKey("VillaNro");

                    b.HasIndex("VillaId");

                    b.ToTable("VillaNumbers");
                });

            modelBuilder.Entity("MagicVilla_API.Models.VillaNumber", b =>
                {
                    b.HasOne("MagicVilla_API.Models.Villa", "Villa")
                        .WithMany()
                        .HasForeignKey("VillaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Villa");
                });
#pragma warning restore 612, 618
        }
    }
}
