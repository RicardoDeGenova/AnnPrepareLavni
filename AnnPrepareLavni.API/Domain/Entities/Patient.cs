using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AnnPrepareLavni.API.Domain.Entities;

public enum Gender
{
    Male = 0,
    Female = 1
}

public enum MeasureType
{
    Imperial = 0,
    Metric = 1
}

public enum HighestEducation
{
    None = 0,
    PrimarySchool = 1,
    MiddleSchool = 2,
    HighSchoolOrGED = 3,
    SomeCollege = 4,
    College = 5,
    MastersDegree = 6,
    Doctorate = 7,
}

public class Patient
{
    public Patient()
    {
        CreatedAt = DateTime.Now;
    }

    public required Guid Id { get; init; }
    public required string FirstName { get; set; } = string.Empty;
    public required string LastName { get; set; } = string.Empty;
    public required DateTimeOffset DateOfBirth { get; set; }
    public required Gender Gender { get; set; }
    public required float HeightInMeters { get; set; }
    public required float WeightInKilograms { get; set; }
    public int FamilySize { get; set; }
    public HighestEducation HighestEducation { get; set; }
    public List<MedicalCondition> MedicalConditions { get; set; } = [];
    public DateTimeOffset CreatedAt { get; init; }
    public Address Address { get; set; }

    public string FullName => $"{FirstName} {LastName}";
    public int Age => DateTime.Now.Year - DateOfBirth.Year;
}
