using AnnPrepareLavni.API.Domain.Entities;

namespace AnnPrepareLavni.API.Contracts;

public class PatientResponse
{
    public required Guid Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; } 
    public required DateTimeOffset DateOfBirth { get; init; }
    public required Gender Gender { get; init; }
    public required float HeightInMeters { get; init; }
    public required float WeightInKilograms { get; init; }
    public required int FamilySize { get; init; }
    public required HighestEducation HighestEducation { get; init; }
    public required List<MedicalCondition> MedicalConditions { get; init; } 
    public required DateTimeOffset CreatedAt { get; init; }
    public required Address Address { get; init; }
}