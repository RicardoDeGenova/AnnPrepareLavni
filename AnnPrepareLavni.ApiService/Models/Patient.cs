using AnnPrepareLavni.ApiService.Models.Enums;
using AnnPrepareLavni.ApiService.Utils.Extensions;

namespace AnnPrepareLavni.ApiService.Models;

public class Patient
{
    public Guid Id { get; set; }
    public int PatientNo { get; set; }    
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public float HeightInMeters { get; set; }
    public float WeightInKilograms { get; set; }
    public int FamilySize { get; set; }
    public HighestEducation HighestEducation { get; set; }
    public string SurgicalProceduresNotes { get; set; } = string.Empty;
    public string FamilyHistoryNotes { get; set; } = string.Empty;
    public string AllergiesNotes { get; set; } = string.Empty;
    public string ProfileNotes { get; set; } = string.Empty;

    public Address? Address { get; set; }
    public ICollection<MedicalCondition> MedicalConditions { get; } = [];
    public ICollection<Prescription> Prescriptions { get; } = [];
    public ICollection<Triage> Triages { get; } = [];
    public ICollection<Appointment> Appointments { get; set; } = [];
    public ICollection<Visit> Visits { get; set; } = [];

    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public string FullName => $"{FirstName} {LastName}";
    public int Age => DateOfBirth.GetAge();
}