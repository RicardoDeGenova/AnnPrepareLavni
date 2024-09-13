
using AnnPrepareLavni.ApiService.Models.Enums;
using AnnPrepareLavni.ApiService.Utils.Extensions;

namespace AnnPrepareLavni.ApiService.Models;

public class Patient 
{
    public Patient()
    {
        CreatedAt = DateTime.Now;
    }

    public Guid Id { get; init; }
    public int PatientNo { get; init; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public float HeightInMeters { get; set; }
    public float WeightInKilograms { get; set; }
    public int FamilySize { get; set; }
    public HighestEducation HighestEducation { get; set; }
    public List<MedicalCondition> MedicalConditions { get; set; }
    public string SurgicalProceduresNotes { get; set; }
    public string FamilyHistoryNotes { get; set; }
    public string AllergiesNotes { get; set; }
    public string ProfileNotes { get; set; }

    public Guid AddressId { get; set; }
    public Address Address { get; set; }

    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public string FullName => $"{FirstName} {LastName}";
    public int Age => DateOfBirth.GetAge();
}
