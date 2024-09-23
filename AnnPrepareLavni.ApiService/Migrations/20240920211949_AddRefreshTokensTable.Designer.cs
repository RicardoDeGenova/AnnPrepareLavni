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
    [Migration("20240920211949_AddRefreshTokensTable")]
    partial class AddRefreshTokensTable
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
                        .HasColumnType("TEXT");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PatientId")
                        .HasColumnType("TEXT");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Street1")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Street2")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PatientId")
                        .IsUnique();

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.Appointment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("DoctorId")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PatientId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ScheduledAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DoctorId");

                    b.HasIndex("PatientId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.Department", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Departments");
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

                    b.Property<Guid>("PatientId")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("StartedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("StoppedAt")
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

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("DosageForm")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Strength")
                        .HasColumnType("REAL");

                    b.Property<int>("StrengthUnit")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Medications");
                });

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.Patient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("AllergiesNotes")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("TEXT");

                    b.Property<string>("FamilyHistoryNotes")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("FamilySize")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Gender")
                        .HasColumnType("INTEGER");

                    b.Property<float>("HeightInMeters")
                        .HasColumnType("REAL");

                    b.Property<int>("HighestEducation")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("PatientNo")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProfileNotes")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SurgicalProceduresNotes")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<float>("WeightInKilograms")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.Prescription", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DatePrescribed")
                        .HasColumnType("TEXT");

                    b.Property<string>("DosageInstructions")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("MedicationId")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PatientId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PrescriberId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Refills")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("VisitId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MedicationId");

                    b.HasIndex("PatientId");

                    b.HasIndex("PrescriberId");

                    b.HasIndex("VisitId");

                    b.ToTable("Prescriptions");
                });

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("DeviceInfo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ReplacedByToken")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.Triage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("BloodPressure")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Complaint")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("NurseId")
                        .HasColumnType("TEXT");

                    b.Property<double>("OxygenSaturationLevel")
                        .HasColumnType("REAL");

                    b.Property<Guid>("PatientId")
                        .HasColumnType("TEXT");

                    b.Property<string>("SugarLevel")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("TemperatureInCelsius")
                        .HasColumnType("REAL");

                    b.Property<Guid?>("VisitId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NurseId");

                    b.HasIndex("PatientId");

                    b.HasIndex("VisitId");

                    b.ToTable("Triages");
                });

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("DepartmentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Language")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Role")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.Visit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("AdmittedAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("AssignedDoctorId")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DischargedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PatientId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AssignedDoctorId");

                    b.HasIndex("PatientId");

                    b.ToTable("Visits");
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

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.Appointment", b =>
                {
                    b.HasOne("AnnPrepareLavni.ApiService.Models.User", "Doctor")
                        .WithMany("Appointments")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AnnPrepareLavni.ApiService.Models.Patient", "Patient")
                        .WithMany("Appointments")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");

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

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.Prescription", b =>
                {
                    b.HasOne("AnnPrepareLavni.ApiService.Models.Medication", null)
                        .WithMany("Prescriptions")
                        .HasForeignKey("MedicationId");

                    b.HasOne("AnnPrepareLavni.ApiService.Models.Patient", "Patient")
                        .WithMany("Prescriptions")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AnnPrepareLavni.ApiService.Models.User", "Prescriber")
                        .WithMany("Prescriptions")
                        .HasForeignKey("PrescriberId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("AnnPrepareLavni.ApiService.Models.Visit", null)
                        .WithMany("Prescriptions")
                        .HasForeignKey("VisitId");

                    b.Navigation("Patient");

                    b.Navigation("Prescriber");
                });

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.Triage", b =>
                {
                    b.HasOne("AnnPrepareLavni.ApiService.Models.User", "Nurse")
                        .WithMany("Triages")
                        .HasForeignKey("NurseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("AnnPrepareLavni.ApiService.Models.Patient", "Patient")
                        .WithMany("Triages")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AnnPrepareLavni.ApiService.Models.Visit", null)
                        .WithMany("Triages")
                        .HasForeignKey("VisitId");

                    b.Navigation("Nurse");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.User", b =>
                {
                    b.HasOne("AnnPrepareLavni.ApiService.Models.Department", null)
                        .WithMany("Staff")
                        .HasForeignKey("DepartmentId");
                });

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.Visit", b =>
                {
                    b.HasOne("AnnPrepareLavni.ApiService.Models.User", "AssignedDoctor")
                        .WithMany()
                        .HasForeignKey("AssignedDoctorId");

                    b.HasOne("AnnPrepareLavni.ApiService.Models.Patient", "Patient")
                        .WithMany("Visits")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AssignedDoctor");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.Department", b =>
                {
                    b.Navigation("Staff");
                });

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.Medication", b =>
                {
                    b.Navigation("Prescriptions");
                });

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.Patient", b =>
                {
                    b.Navigation("Address");

                    b.Navigation("Appointments");

                    b.Navigation("MedicalConditions");

                    b.Navigation("Prescriptions");

                    b.Navigation("Triages");

                    b.Navigation("Visits");
                });

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.User", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Prescriptions");

                    b.Navigation("Triages");
                });

            modelBuilder.Entity("AnnPrepareLavni.ApiService.Models.Visit", b =>
                {
                    b.Navigation("Prescriptions");

                    b.Navigation("Triages");
                });
#pragma warning restore 612, 618
        }
    }
}