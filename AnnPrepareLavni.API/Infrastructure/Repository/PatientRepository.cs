using AnnPrepareLavni.API.Infrastructure.Database;
using AnnPrepareLavni.Domain.Abstract.Domain.Entities;
using Dapper;

namespace AnnPrepareLavni.API.Infrastructure.Repository;

public class PatientRepository : IPatientRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public PatientRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IEnumerable<IPatient>> GetAllPatientsAsync()
    {
        var sql = @"
                SELECT p.*, a.*, mc.*
                FROM Patient p
                INNER JOIN Address a ON p.AddressId = a.Id
                LEFT JOIN PatientMedicalCondition pmc ON pmc.PatientId = p.Id
                LEFT JOIN MedicalCondition mc ON mc.Id = pmc.MedicalConditionId";

        using var connection = _dbConnectionFactory.CreateConnection();
        var patientDictionary = new Dictionary<PatientId, IPatient>();
        
        var patients = await connection.QueryAsync<IPatient, IAddress, IMedicalCondition, IPatient>(
            sql,
            (patient, address, medicalCondition) =>
            {
                if (!patientDictionary.TryGetValue(patient.Id, out var currentPatient))
                {
                    currentPatient = patient;
                    currentPatient.Address = address;
                    currentPatient.MedicalConditions = new List<IMedicalCondition>();
                    patientDictionary.Add(currentPatient.Id, currentPatient);
                }

                if (medicalCondition != null)
                {
                    currentPatient.MedicalConditions.Add(medicalCondition);
                }

                return currentPatient;
            },
            splitOn: "Id,Id,Id");

        return patients.Distinct().ToList();
    }
}


public interface IPatientRepository
{
    //Task<bool> DeletePatientAsync(int id);
    //Task<bool> UpdatePatientAsync(Patient patient);
    //Task<int> CreatePatientAsync(Patient patient);
    //Task<Patient> GetPatientByIdAsync(int id);
    Task<IEnumerable<IPatient>> GetAllPatientsAsync();
}