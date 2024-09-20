using AnnPrepareLavni.ApiService.Data;
using AnnPrepareLavni.ApiService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AnnPrepareLavni.ApiService.Features.Patient;

public interface IPatientService : IGenericService<Models.Patient>
{
    public Task<bool> CreateNewAddressAsync(Models.Address address, Guid patientId);
    public Task<Models.Patient?> GetByPatientNo(int patientNo);
}

public class PatientService : IPatientService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PatientService> _logger;

    public PatientService(ApplicationDbContext context, ILogger<PatientService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Models.Patient?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            _logger.LogWarning($"{nameof(GetByIdAsync)} called with empty GUID.");
            return null;
        }

        try
        {
            return await _context.Patients
                .Include(p => p.Address)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient with ID {PatientId}", id);
            throw;
        }
    }

    public async Task<Models.Patient?> GetByPatientNo(int patientNo)
    {
        if (patientNo == 0)
        {
            _logger.LogWarning($"{nameof(GetByPatientNo)} called with empty patientNo.");
            return null;
        }

        try
        {
            return await _context.Patients
                .Include(p => p.Address)
                .FirstOrDefaultAsync(p => p.PatientNo == patientNo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient with ID {PatientId}", patientNo);
            throw;
        }
    }

    public async Task<IEnumerable<Models.Patient>> GetAllAsync()
    {
        try
        {
            return await _context.Patients
                .Include(p => p.Address)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all patients.");
            throw;
        }
    }

    public async Task<bool> CreateAsync(Models.Patient patient)
    {
        if (patient == null)
        {
            _logger.LogWarning("CreateAsync called with null entity.");
            return false;
        }

        try
        {
            patient.Id = Guid.NewGuid();
            patient.ModifiedAt = DateTimeOffset.UtcNow;

            if (patient.Address != null)
            {
                patient.Address.Id = Guid.NewGuid();
                patient.Address.PatientId = patient.Id;
                await _context.Addresses.AddAsync(patient.Address);
            }

            await _context.Patients.AddAsync(patient);
            var created = await _context.SaveChangesAsync();
            return created > 0;
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Database update exception during CreateAsync.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating patient.");
            throw;
        }
    }

    public async Task<bool> CreateNewAddressAsync(Models.Address address, Guid patientId)
    {
        if (address == null)
        {
            _logger.LogWarning("CreateNewAddressAsync called with null entity.");
            return false;
        }

        if (patientId == Guid.Empty)
        {
            _logger.LogWarning("CreateNewAddressAsync called with invalid patient ID.");
            return false;
        }

        address.Id = Guid.NewGuid();
        address.PatientId = patientId;
        
        try
        {
            await _context.Addresses.AddAsync(address);
            var created = await _context.SaveChangesAsync();
            return created > 0;
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Database update exception during CreateAsync.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating patient.");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(Models.Patient entity)
    {
        if (entity == null)
        {
            _logger.LogWarning("UpdateAsync called with null entity.");
            return false;
        }

        entity.ModifiedAt = DateTimeOffset.Now;

        try
        {
            _context.Patients.Update(entity);
            var updated = await _context.SaveChangesAsync();
            return updated > 0;
        }
        catch (DbUpdateConcurrencyException concurrencyEx)
        {
            _logger.LogError(concurrencyEx, "Concurrency exception during UpdateAsync.");
            throw;
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Database update exception during UpdateAsync.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating patient.");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            _logger.LogWarning("DeleteAsync called with empty GUID.");
            return false;
        }

        try
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                _logger.LogWarning("Patient with ID {PatientId} not found for deletion.", id);
                return false;
            }

            _context.Patients.Remove(patient);
            var deleted = await _context.SaveChangesAsync();
            return deleted > 0;
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Database update exception during DeleteAsync.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting patient.");
            throw;
        }
    }
}
