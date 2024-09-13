
using AnnPrepareLavni.ApiService.Models.Enums;
using AnnPrepareLavni.ApiService.Utils.Extensions;

namespace AnnPrepareLavni.ApiService.Models;

public class Patient 
{
    public Patient()
    {
        CreatedAt = DateTime.Now;
    }

    public required Guid Id { get; init; }
    public int PatientNo { get; init; }
    public required string FirstName { get; set; } = string.Empty;
    public required string LastName { get; set; } = string.Empty;
    public required DateTime DateOfBirth { get; set; }
    public required Gender Gender { get; set; }
    public required float HeightInMeters { get; set; }
    public required float WeightInKilograms { get; set; }
    public int FamilySize { get; set; }
    public HighestEducation HighestEducation { get; set; }
    public List<MedicalCondition> MedicalConditions { get; set; }
    public string SurgicalProceduresNotes { get; set; }
    public string FamilyHistoryNotes { get; set; }
    public string AllergiesNotes { get; set; }

    public Guid AddressId { get; set; }
    public Address Address { get; set; }

    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public string FullName => $"{FirstName} {LastName}";
    public int Age => DateOfBirth.GetAge();
}
