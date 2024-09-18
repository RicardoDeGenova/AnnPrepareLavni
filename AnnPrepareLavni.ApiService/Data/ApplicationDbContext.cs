using AnnPrepareLavni.ApiService.Models;
using Microsoft.EntityFrameworkCore;

namespace AnnPrepareLavni.ApiService.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<MedicalCondition> MedicalConditions { get; set; }
    public DbSet<Medication> Medications { get; set; }
    public DbSet<MedicationName> MedicationNames { get; set; }

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
            .HasMany(p => p.Medications)
            .WithOne(m => m.Patient)
            .HasForeignKey(m => m.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Medication>()
            .HasOne(pm => pm.MedicationName)
            .WithMany(m => m.Medications)
            .HasForeignKey(m => m.MedicationNameId);

        modelBuilder.Entity<Patient>()
            .Ignore(p => p.FullName)
            .Ignore(p => p.Age);

    }
}