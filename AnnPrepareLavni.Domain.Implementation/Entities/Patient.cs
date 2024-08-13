using AnnPrepareLavni.Domain.Abstract.Domain.Entities;
using AnnPrepareLavni.Domain.Abstract.Enums;

namespace AnnPrepareLavni.Domain.Implementation.Entities;

public class Patient : IPatient
{
    public Patient()
    {
        CreatedAt = DateTime.Now;
    }

    public required PatientId Id { get; init; }
    public required string FirstName { get; set; } = string.Empty;
    public required string LastName { get; set; } = string.Empty;
    public required DateTimeOffset DateOfBirth { get; set; }
    public required Gender Gender { get; set; }
    public required float HeightInMeters { get; set; }
    public required float WeightInKilograms { get; set; }
    public int FamilySize { get; set; }
    public HighestEducation HighestEducation { get; set; }
    public List<IMedicalCondition> MedicalConditions { get; set; } = [];
    public DateTimeOffset CreatedAt { get; init; }
    public IAddress Address { get; set; }

    public string FullName => $"{FirstName} {LastName}";
    public int Age => DateTime.Now.Year - DateOfBirth.Year;
}
