using AnnPrepareLavni.API.Contracts.Requests;
using AnnPrepareLavni.API.Contracts.Responses;
using AnnPrepareLavni.BusinessLogic.Builders;
using AnnPrepareLavni.Domain.Abstract.Domain.Entities;

namespace AnnPrepareLavni.API.Contracts;

public static class PatientContractMappings
{
    public static IPatient MapToPatient(this CreatePatientRequest request)
    {
        return PatientBuilder.CreateWith(
            PatientId.New(),
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.Gender,
            request.HeightInMeters,
            request.WeightInKilograms);
    }

    public static PatientResponse MapToResponse(this IPatient patient) => new()
    {
        Id = patient.Id,
        FirstName = patient.FirstName,
        LastName = patient.LastName,
        DateOfBirth = patient.DateOfBirth,
        Gender = patient.Gender,
        HeightInMeters = patient.HeightInMeters,
        WeightInKilograms = patient.WeightInKilograms,
        FamilySize = patient.FamilySize,
        HighestEducation = patient.HighestEducation,
        MedicalConditions = patient.MedicalConditions,
        CreatedAt = patient.CreatedAt,
        Address = patient.Address,
    };

    public static PatientsResponse MapToResponse(this IEnumerable<IPatient> patient) => new()
    {
        Patients = patient.Select(MapToResponse)
    };
}
