using AnnPrepareLavni.API.Adresses;
using AnnPrepareLavni.API.Contracts.Requests;
using AnnPrepareLavni.API.Contracts.Responses;
using AnnPrepareLavni.API.Patients;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AnnPrepareLavni.API.Contracts;

public static class PatientContractMappings
{
    public static Patient MapToPatient(this CreatePatientRequest request) => new()
    {
        Id = Guid.NewGuid(),
        DateOfBirth = request.DateOfBirth,
        FirstName = request.FirstName,
        LastName = request.LastName,
        Gender = request.Gender,
        HeightInMeters = request.HeightInMeters,
        WeightInKilograms = request.WeightInKilograms,        
    };

    public static PatientResponse MapToResponse(this Patient patient) => new()
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

    public static PatientsResponse MapToResponse(this IEnumerable<Patient> patient) => new()
    {
        Items = patient.Select(MapToResponse)
    };
}
