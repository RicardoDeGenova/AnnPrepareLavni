using AnnPrepareLavni.API.Domain.Entities;
using AnnPrepareLavni.API.Infrastructure.Database;
using Dapper;

namespace AnnPrepareLavni.API.Infrastructure.Repository;

public class PatientRepository : IPatientRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public PatientRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
    {
        var sql = @"
                SELECT p.*, a.*, mc.*
                FROM Patient p
                INNER JOIN Address a ON p.AddressId = a.Id
                LEFT JOIN PatientMedicalCondition pmc ON pmc.PatientId = p.Id
                LEFT JOIN MedicalCondition mc ON mc.Id = pmc.MedicalConditionId";

        using var connection = _dbConnectionFactory.CreateConnection();
        var patientDictionary = new Dictionary<Guid, Patient>();
        
        var patients = await connection.QueryAsync<Patient, Address, MedicalCondition, Patient>(
            sql,
            (patient, address, medicalCondition) =>
            {
                if (!patientDictionary.TryGetValue(patient.Id, out var currentPatient))
                {
                    currentPatient = patient;
                    currentPatient.Address = address;
                    currentPatient.MedicalConditions = new List<MedicalCondition>();
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
    Task<IEnumerable<Patient>> GetAllPatientsAsync();
}