using AnnPrepareLavni.ApiService.Models;
using Microsoft.EntityFrameworkCore;

namespace AnnPrepareLavni.ApiService.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Medication> Medications { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<MedicalCondition> MedicalConditions { get; set; }
    public DbSet<Triage> Triages { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Visit> Visits { get; set; } 
    public DbSet<Department> Departments { get; set; } 
    public DbSet<RefreshToken> RefreshTokens { get; set; } 

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>()
                    .HasOne(p => p.Address)
                    .WithOne(a => a.Patient)
                    .HasForeignKey<Address>(a => a.PatientId)
                    .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Patient>()
                    .HasMany(p => p.MedicalConditions)
                    .WithOne(m => m.Patient)
                    .HasForeignKey(m => m.PatientId)
                    .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Patient>()
                    .HasMany(p => p.Prescriptions)
                    .WithOne(m => m.Patient)
                    .HasForeignKey(m => m.PatientId)
                    .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Patient>()
                    .Ignore(p => p.FullName)
                    .Ignore(p => p.Age);

        modelBuilder.Entity<User>()
                    .HasMany(u => u.Prescriptions)
                    .WithOne(p => p.Prescriber)
                    .HasForeignKey(p => p.PrescriberId)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Triage>()
                    .HasOne(t => t.Nurse)
                    .WithMany(u => u.Triages)
                    .HasForeignKey(t => t.NurseId)
                    .OnDelete(DeleteBehavior.Restrict);
    }
}