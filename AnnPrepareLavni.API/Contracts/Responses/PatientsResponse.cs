namespace AnnPrepareLavni.API.Contracts.Responses;

public class PatientsResponse
{
    public required IEnumerable<PatientResponse> Items { get; init; } = Enumerable.Empty<PatientResponse>();
}
