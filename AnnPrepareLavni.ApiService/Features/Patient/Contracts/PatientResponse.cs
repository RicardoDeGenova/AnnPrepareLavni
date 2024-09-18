using AnnPrepareLavni.ApiService.Models.Enums;
using AnnPrepareLavni.ApiService.Models;
using AnnPrepareLavni.ApiService.Features.Address.Contracts;
using AnnPrepareLavni.ApiService.Features.MedicalCondition.Contracts;
using System.ComponentModel.DataAnnotations;

namespace AnnPrepareLavni.ApiService.Features.Patient.Contracts;

public class PatientResponse
{
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
    public AddressResponse? Address { get; set; }
    public ICollection<MedicalConditionResponse>? MedicalConditions { get; set; }

    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

}
