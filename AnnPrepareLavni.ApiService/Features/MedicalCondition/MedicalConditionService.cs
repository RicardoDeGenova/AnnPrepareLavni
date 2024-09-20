using AnnPrepareLavni.ApiService.Data;
using AnnPrepareLavni.ApiService.Infrastructure;
using AnnPrepareLavni.ApiService.Models;
using Microsoft.EntityFrameworkCore;

namespace AnnPrepareLavni.ApiService.Features.MedicalCondition;

public interface IMedicalConditionService : IGenericService<Models.MedicalCondition>
{
    public Task<IEnumerable<Models.MedicalCondition>> GetByPatientIdAsync(Guid patientId);
}

public class MedicalConditionService : IMedicalConditionService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<MedicalConditionService> _logger;

    public MedicalConditionService(ApplicationDbContext context, ILogger<MedicalConditionService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Models.MedicalCondition?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            _logger.LogWarning($"{nameof(GetByIdAsync)} called with empty GUID.");
            return null;
        }

        try
        {
            return await _context.MedicalConditions
                .Include(m => m.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving medical condition with ID {MedicalConditionId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Models.MedicalCondition>> GetAllAsync()
    {
        try
        {
            return await _context.MedicalConditions
                .Include(m => m.Patient)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all medical conditions.");
            throw;
        }
    }

    public async Task<IEnumerable<Models.MedicalCondition>> GetByPatientIdAsync(Guid patientId)
    {
        if (patientId == Guid.Empty)
        {
            _logger.LogWarning($"{nameof(GetByPatientIdAsync)} called with empty patient ID.");
            return Enumerable.Empty<Models.MedicalCondition>();
        }

        try
        {
            return await _context.MedicalConditions
                .Include(m => m.Patient)
                .Where(m => m.PatientId == patientId)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving medical conditions for patient ID {PatientId}", patientId);
            throw;
        }
    }

    public async Task<bool> CreateAsync(Models.MedicalCondition entity)
    {
        if (entity == null)
        {
            _logger.LogWarning("CreateAsync called with null entity.");
            return false;
        }

        try
        {
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTimeOffset.UtcNow;
            entity.ModifiedAt = DateTimeOffset.UtcNow;

            await _context.MedicalConditions.AddAsync(entity);
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
            _logger.LogError(ex, "Error creating medical condition.");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(Models.MedicalCondition entity)
    {
        if (entity == null)
        {
            _logger.LogWarning("UpdateAsync called with null entity.");
            return false;
        }

        entity.ModifiedAt = DateTimeOffset.UtcNow;

        try
        {
            _context.MedicalConditions.Update(entity);
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
            _logger.LogError(ex, "Error updating medical condition.");
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
            var medicalCondition = await _context.MedicalConditions.FindAsync(id);
            if (medicalCondition == null)
            {
                _logger.LogWarning("Medical condition with ID {MedicalConditionId} not found for deletion.", id);
                return false;
            }

            _context.MedicalConditions.Remove(medicalCondition);
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
            _logger.LogError(ex, "Error deleting medical condition.");
            throw;
        }
    }
}
