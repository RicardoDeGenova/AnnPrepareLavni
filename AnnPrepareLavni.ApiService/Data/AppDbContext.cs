using AnnPrepareLavni.ApiService.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AnnPrepareLavni.ApiService.Data;

public class AppDbContext : DbContext
{
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<MedicalCondition> MedicalConditions { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure relationships
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
            .Ignore(p => p.FullName)
            .Ignore(p => p.Age);

        modelBuilder.Entity<Patient>()
            .Ignore(p => p.FullName)
            .Ignore(p => p.Age);
    }
}