﻿// <auto-generated />
using System;
using AnnPrepareLavni.ApiService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AnnPrepareLavni.ApiService.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PatientId")
                        .HasColumnType("TEXT");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Street1")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Street2")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PatientId")
                        .IsUnique();

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.MedicalCondition", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("ConditionType")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PatientId")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("StartedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("StoppedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.ToTable("MedicalConditions");
                });

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.Patient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("AllergiesNotes")
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("TEXT");

                    b.Property<string>("FamilyHistoryNotes")
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.Property<int>("FamilySize")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int>("Gender")
                        .HasColumnType("INTEGER");

                    b.Property<float>("HeightInMeters")
                        .HasColumnType("REAL");

                    b.Property<int>("HighestEducation")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("PatientNo")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProfileNotes")
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.Property<string>("SurgicalProceduresNotes")
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.Property<float>("WeightInKilograms")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.Address", b =>
                {
                    b.HasOne("AnnPrepareLavni.ApiService.Models.Patient", "Patient")
                        .WithOne("Address")
                        .HasForeignKey("AnnPrepareLavni.ApiService.Models.Address", "PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.MedicalCondition", b =>
                {
                    b.HasOne("AnnPrepareLavni.ApiService.Models.Patient", "Patient")
                        .WithMany("MedicalConditions")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.Patient", b =>
                {
                    b.Navigation("Address");

                    b.Navigation("MedicalConditions");
                });
#pragma warning restore 612, 618
        }
    }
}
