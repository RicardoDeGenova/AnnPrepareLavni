using AnnPrepareLavni.Domain.Abstract.Enums;

namespace AnnPrepareLavni.API.Contracts.Requests;

public class CreatePatientRequest
{
    public required string FirstName { get; set; } = string.Empty;
    public required string LastName { get; set; } = string.Empty;
    public required DateTimeOffset DateOfBirth { get; set; }
    public required Gender Gender { get; set; }
    public required float HeightInMeters { get; set; }
    public required float WeightInKilograms { get; set; }
}
