﻿// <auto-generated />
using System;
using AnnPrepareLavni.ApiService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AnnPrepareLavni.ApiService.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240918223152_Script0003")]
    partial class Script0003
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("PatientId")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("StartedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("StoppedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.ToTable("MedicalConditions");
                });

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.Medication", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("FrequencyType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FrequencyWhenAtRegularIntervals")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FrequencyWhenOnSpecificDays")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("MedicationNameId")
                        .HasColumnType("TEXT");

                    b.Property<int>("MedicationType")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("PatientId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PrescriberId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<double>("Strength")
                        .HasColumnType("REAL");

                    b.Property<int>("StrengthType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TimesToTakeMedicine")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MedicationNameId");

                    b.HasIndex("PatientId");

                    b.HasIndex("PrescriberId");

                    b.ToTable("Medications");
                });

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.MedicationName", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("Language")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("MedicationNames");
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

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Language")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Occupation")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Role")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("User");
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
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.Medication", b =>
                {
                    b.HasOne("AnnPrepareLavni.ApiService.Models.MedicationName", "MedicationName")
                        .WithMany("Medications")
                        .HasForeignKey("MedicationNameId");

                    b.HasOne("AnnPrepareLavni.ApiService.Models.Patient", "Patient")
                        .WithMany("Medications")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AnnPrepareLavni.ApiService.Models.User", "Prescriber")
                        .WithMany()
                        .HasForeignKey("PrescriberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MedicationName");

                    b.Navigation("Patient");

                    b.Navigation("Prescriber");
                });

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.MedicationName", b =>
                {
                    b.Navigation("Medications");
                });

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.Patient", b =>
                {
                    b.Navigation("Address");

                    b.Navigation("MedicalConditions");

                    b.Navigation("Medications");
                });
#pragma warning restore 612, 618
        }
    }
}
