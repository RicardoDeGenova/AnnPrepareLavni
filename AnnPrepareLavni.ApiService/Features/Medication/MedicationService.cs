using AnnPrepareLavni.ApiService.Data;
using AnnPrepareLavni.ApiService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AnnPrepareLavni.ApiService.Features.Medication;
public interface IMedicationService : IGenericService<Models.Medication>
{
    Task<IEnumerable<Models.Medication>> GetByNameAsync(string name);
}

public class MedicationService : IMedicationService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<MedicationService> _logger;

    public MedicationService(ApplicationDbContext context, ILogger<MedicationService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Models.Medication?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            _logger.LogWarning($"{nameof(GetByIdAsync)} called with empty GUID.");
            return null;
        }

        try
        {
            return await _context.Medications
                .Include(m => m.Prescriptions)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving medication with ID {MedicationId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Models.Medication>> GetAllAsync()
    {
        try
        {
            return await _context.Medications
                .Include(m => m.Prescriptions)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all medications.");
            throw;
        }
    }

    public async Task<IEnumerable<Models.Medication>> GetByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            _logger.LogWarning($"{nameof(GetByNameAsync)} called with null or empty name.");
            return Enumerable.Empty<Models.Medication>();
        }

        try
        {
            return await _context.Medications
                .Include(m => m.Prescriptions)
                .Where(m => m.Name.Contains(name))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving medications with name {MedicationName}", name);
            throw;
        }
    }

    public async Task<bool> CreateAsync(Models.Medication entity)
    {
        if (entity == null)
        {
            _logger.LogWarning("CreateAsync called with null entity.");
            return false;
        }

        try
        {
            entity.Id = Guid.NewGuid();
            await _context.Medications.AddAsync(entity);
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
            _logger.LogError(ex, "Error creating medication.");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(Models.Medication entity)
    {
        if (entity == null)
        {
            _logger.LogWarning("UpdateAsync called with null entity.");
            return false;
        }

        try
        {
            _context.Medications.Update(entity);
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
            _logger.LogError(ex, "Error updating medication.");
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
            var medication = await _context.Medications.FindAsync(id);
            if (medication == null)
            {
                _logger.LogWarning("Medication with ID {MedicationId} not found for deletion.", id);
                return false;
            }

            _context.Medications.Remove(medication);
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
            _logger.LogError(ex, "Error deleting medication.");
            throw;
        }
    }
}
