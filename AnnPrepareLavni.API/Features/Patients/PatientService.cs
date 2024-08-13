using FluentValidation;
using AnnPrepareLavni.API.Infrastructure.Repository;
using AnnPrepareLavni.Domain.Abstract.Domain.Entities;

namespace AnnPrepareLavni.API.Endpoints.Patients;

public class PatientService : IPatientService
{
    private readonly IValidator<IPatient> _patientValidator;
    private readonly IPatientRepository _patientRepository;

    public PatientService(IValidator<IPatient> patientValidator, IPatientRepository patientRepository)
    {
        _patientValidator = patientValidator;
        _patientRepository = patientRepository;
    }

    public async Task<IEnumerable<IPatient>> GetAllAsync()
    {
        return await _patientRepository.GetAllPatientsAsync();
    }

}

public interface IPatientService
{
    Task<IEnumerable<IPatient>> GetAllAsync();
}
