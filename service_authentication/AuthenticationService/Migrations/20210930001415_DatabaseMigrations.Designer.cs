﻿// <auto-generated />
using System;
using AuthenticationService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AuthenticationService.Migrations
{
    [DbContext(typeof(AuthenticationServiceContext))]
    [Migration("20210930001415_DatabaseMigrations")]
    partial class DatabaseMigrations
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DomainModel.Authentication", b =>
                {
                    b.Property<int>("AuthenticationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AuthenticationID");

                    b.ToTable("Authentication");
                });

            modelBuilder.Entity("DomainModel.Medic", b =>
                {
                    b.Property<int>("MedicID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AuthenticationID")
                        .HasColumnType("int");

                    b.Property<string>("Rotation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Semester")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("MedicID");

                    b.HasIndex("AuthenticationID");

                    b.HasIndex("UserID");

                    b.ToTable("Medic");
                });

            modelBuilder.Entity("DomainModel.Pacient", b =>
                {
                    b.Property<int>("PacientID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Bloodgroup")
                        .HasColumnType("int");

                    b.Property<int?>("Rh")
                        .HasColumnType("int");

                    b.Property<int?>("Sex")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("PacientID");

                    b.HasIndex("UserID");

                    b.ToTable("Pacient");
                });

            modelBuilder.Entity("DomainModel.Procedure", b =>
                {
                    b.Property<int>("ProcedureID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Asa")
                        .HasColumnType("int");

                    b.Property<int>("MedicID")
                        .HasColumnType("int");

                    b.Property<int>("PacientID")
                        .HasColumnType("int");

                    b.Property<string>("PatientStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProcedureName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("VideoRecord")
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("_pacientID")
                        .HasColumnType("int");

                    b.HasKey("ProcedureID");

                    b.HasIndex("_pacientID");

                    b.ToTable("Procedure");
                });

            modelBuilder.Entity("DomainModel.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecondName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID");

                    b.ToTable("User");
                });

            modelBuilder.Entity("DomainModel.Medic", b =>
                {
                    b.HasOne("DomainModel.Authentication", "AuthenticationData")
                        .WithMany()
                        .HasForeignKey("AuthenticationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DomainModel.User", "MedicData")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AuthenticationData");

                    b.Navigation("MedicData");
                });

            modelBuilder.Entity("DomainModel.Pacient", b =>
                {
                    b.HasOne("DomainModel.User", "PacientInfo")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PacientInfo");
                });

            modelBuilder.Entity("DomainModel.Procedure", b =>
                {
                    b.HasOne("DomainModel.Pacient", "Pacient")
                        .WithMany()
                        .HasForeignKey("_pacientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pacient");
                });
#pragma warning restore 612, 618
        }
    }
}