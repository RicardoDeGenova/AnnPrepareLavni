using AnnPrepareLavni.API.Validation;
using AnnPrepareLavni.API.Contracts;
using AnnPrepareLavni.API.Contracts.Responses;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using FluentValidation;
using System.Collections.Concurrent;

namespace AnnPrepareLavni.API.Patients;

public class PatientService : IPatientService
{
    private readonly IValidator<Patient> _patientValidator;
    private readonly ConcurrentDictionary<Guid, Patient> _patients = new();

    public PatientService(IValidator<Patient> movieValidator)
    {
        _patientValidator = movieValidator;
    }

    public Task<Result<Patient, ValidationFailed>> CreateAsync(Patient patient)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Patient>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Patient>>(_patients.Values);
    }

    public Task<Patient?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Patient?, ValidationFailed>> UpdateAsync(Patient patient)
    {
        throw new NotImplementedException();
    }
}

public interface IPatientService
{
    Task<Result<Patient, ValidationFailed>> CreateAsync(Patient patient);

    Task<Patient?> GetByIdAsync(Guid id);
    //GetByName?
    Task<IEnumerable<Patient>> GetAllAsync();
    Task<Result<Patient?, ValidationFailed>> UpdateAsync(Patient patient);

    Task<bool> DeleteByIdAsync(Guid id);
}
