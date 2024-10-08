﻿using AnnPrepareLavni.ApiService.Models.Enums;
using AnnPrepareLavni.ApiService.Features.Address.Contracts;
using AnnPrepareLavni.ApiService.Features.MedicalCondition.Contracts;

namespace AnnPrepareLavni.ApiService.Features.Patient.Contracts;

public class PatientRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public float? HeightInMeters { get; set; }
    public float? WeightInKilograms { get; set; }
    public int? FamilySize { get; set; }
    public HighestEducation? HighestEducation { get; set; }
    public string? SurgicalProceduresNotes { get; set; }
    public string? FamilyHistoryNotes { get; set; }
    public string? AllergiesNotes { get; set; }
    public string? ProfileNotes { get; set; }

    public AddressRequest? Address { get; set; }
    public ICollection<MedicalConditionRequest>? MedicalConditions { get; set; }
}
