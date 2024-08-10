using AnnPrepareLavni.API.Contracts;
using AnnPrepareLavni.API.Contracts.Responses;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using FluentValidation;
using System.Collections.Concurrent;
using AnnPrepareLavni.API.Domain.Entities;
using AnnPrepareLavni.API.Common;
using AnnPrepareLavni.API.Infrastructure.Repository;

namespace AnnPrepareLavni.API.Endpoints.Patients;

public class PatientService : IPatientService
{
    private readonly IValidator<Patient> _patientValidator;
    private readonly IPatientRepository _patientRepository;

    public PatientService(IValidator<Patient> patientValidator, IPatientRepository patientRepository)
    {
        _patientValidator = patientValidator;
        _patientRepository = patientRepository;
    }

    public async Task<IEnumerable<Patient>> GetAllAsync()
    {
        return await _patientRepository.GetAllPatientsAsync();
    }

}

public interface IPatientService
{
    Task<IEnumerable<Patient>> GetAllAsync();
}
