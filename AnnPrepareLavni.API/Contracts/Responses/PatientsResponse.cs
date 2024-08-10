namespace AnnPrepareLavni.API.Contracts.Responses;

public class PatientsResponse
{
    public required IEnumerable<PatientResponse> Patients { get; init; } = Enumerable.Empty<PatientResponse>();
}
