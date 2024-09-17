using Riok.Mapperly.Abstractions;

namespace AnnPrepareLavni.ApiService.Features.Patient.Contracts;

[Mapper]
public static partial class PatientMapper
{
    public static partial PatientResponse MapToResponse(Models.Patient patient);
    public static partial IEnumerable<PatientResponse> MapToResponseList(IEnumerable<Models.Patient> patient);
    public static partial Models.Patient MapToPatient(PatientRequest patientRequest);
    public static partial void MapToExistingPatient(PatientRequest patientRequest, Models.Patient patient);
}
