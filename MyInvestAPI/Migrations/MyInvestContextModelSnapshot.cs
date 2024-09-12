﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyInvestAPI.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyInvestAPI.Migrations
{
    [DbContext(typeof(MyInvestContext))]
    partial class MyInvestContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ActivePurse", b =>
                {
                    b.Property<int>("ActivesActive_Id")
                        .HasColumnType("integer");

                    b.Property<int>("PursesPurse_Id")
                        .HasColumnType("integer");

                    b.HasKey("ActivesActive_Id", "PursesPurse_Id");

                    b.HasIndex("PursesPurse_Id");

                    b.ToTable("ActivePurse");
                });

            modelBuilder.Entity("MyInvestAPI.Domain.Active", b =>
                {
                    b.Property<int>("Active_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Active_Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<float>("DYDesiredPercentage")
                        .HasColumnType("real");

                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Active_Id");

                    b.ToTable("Actives");
                });

            modelBuilder.Entity("MyInvestAPI.Domain.Purse", b =>
                {
                    b.Property<int>("Purse_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Purse_Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("User_Id")
                        .HasColumnType("integer");

                    b.HasKey("Purse_Id");

                    b.HasIndex("User_Id");

                    b.ToTable("Purses");
                });

            modelBuilder.Entity("MyInvestAPI.Domain.User", b =>
                {
                    b.Property<int>("User_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("User_Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("character varying(80)");

                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("character varying(80)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("character varying(80)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.HasKey("User_Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ActivePurse", b =>
                {
                    b.HasOne("MyInvestAPI.Domain.Active", null)
                        .WithMany()
                        .HasForeignKey("ActivesActive_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyInvestAPI.Domain.Purse", null)
                        .WithMany()
                        .HasForeignKey("PursesPurse_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyInvestAPI.Domain.Purse", b =>
                {
                    b.HasOne("MyInvestAPI.Domain.User", "User")
                        .WithMany("Purses")
                        .HasForeignKey("User_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MyInvestAPI.Domain.User", b =>
                {
                    b.Navigation("Purses");
                });
#pragma warning restore 612, 618
        }
    }
}
