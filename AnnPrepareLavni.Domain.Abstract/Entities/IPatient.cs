using AnnPrepareLavni.Domain.Abstract.Enums;

namespace AnnPrepareLavni.Domain.Abstract.Domain.Entities;

public record PatientId(Guid Value)
{
    public static PatientId New() => new(Guid.NewGuid());
};

public interface IPatient
{
    PatientId Id { get; init; }
    string FirstName { get; set; }
    string LastName { get; set; }
    DateTimeOffset DateOfBirth { get; set; }
    Gender Gender { get; set; }
    float HeightInMeters { get; set; }
    float WeightInKilograms { get; set; }
    int FamilySize { get; set; }
    HighestEducation HighestEducation { get; set; }
    List<IMedicalCondition> MedicalConditions { get; set; }
    DateTimeOffset CreatedAt { get; init; }
    IAddress Address { get; set; }

    public string FullName { get; }
    public int Age { get; }
}
