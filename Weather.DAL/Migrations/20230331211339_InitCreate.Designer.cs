﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Weather.DAL;
using Weather.Domain.Entities;

#nullable disable

namespace Weather.DAL.Migrations
{
    [DbContext(typeof(AppContextDb))]
    [Migration("20230331211339_InitCreate")]
    partial class InitCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Weather.Domain.Entities.Forecast", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("Id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CreatedAt");

                    b.Property<DateTime?>("DeleteAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("DeleteAt");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UpdatedAt");

                    b.Property<CityForecast>("city")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("city");

                    b.Property<int>("cnt")
                        .HasColumnType("integer")
                        .HasColumnName("cnt");

                    b.Property<List<WeatherItem>>("list")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("list");

                    b.HasKey("Id");

                    b.ToTable("Forecasts");
                });

            modelBuilder.Entity("Weather.Domain.Entities.ForecastUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("Id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<List<CityWeather>>("Cities")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("Cities");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CreatedAt");

                    b.Property<DateTime?>("DeleteAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("DeleteAt");

                    b.Property<int>("UId")
                        .HasColumnType("integer")
                        .HasColumnName("UId");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("UId");

                    b.ToTable("ForecastUsers");
                });

            modelBuilder.Entity("Weather.Domain.Entities.User", b =>
                {
                    b.Property<int>("UId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CreatedAt");

                    b.Property<DateTime?>("DeleteAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("DeleteAt");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Password");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UpdatedAt");

                    b.HasKey("UId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Weather.Domain.Entities.ForecastUser", b =>
                {
                    b.HasOne("Weather.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
