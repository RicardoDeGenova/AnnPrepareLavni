using Riok.Mapperly.Abstractions;

namespace AnnPrepareLavni.ApiService.Features.Patient.Contracts;

[Mapper]
public static partial class PatientMapper
{
    public static partial PatientResponse ToResponse(Models.Patient patient);
    public static partial Models.Patient ToPatient(PatientRequest patientRequest);
    public static partial void MapToExistingPatient(PatientRequest patientRequest, Models.Patient patient);
}
