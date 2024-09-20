using AnnPrepareLavni.ApiService.Data;
using AnnPrepareLavni.ApiService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AnnPrepareLavni.ApiService.Features.Prescription;

public interface IPrescriptionService : IGenericService<Models.Prescription> { }

public class PrescriptionService : IPrescriptionService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PrescriptionService> _logger;

    public PrescriptionService(ApplicationDbContext context, ILogger<PrescriptionService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Models.Prescription?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            _logger.LogWarning($"{nameof(GetByIdAsync)} called with empty GUID.");
            return null;
        }

        try
        {
            return await _context.Prescriptions
                .Include(p => p.Patient)
                .Include(p => p.Prescriber)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving prescription with ID {PrescriptionId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Models.Prescription>> GetAllAsync()
    {
        try
        {
            return await _context.Prescriptions
                .Include(p => p.Patient)
                .Include(p => p.Prescriber)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all prescriptions.");
            throw;
        }
    }

    public async Task<bool> CreateAsync(Models.Prescription prescription)
    {
        if (prescription == null)
        {
            _logger.LogWarning("CreateAsync called with null entity.");
            return false;
        }

        try
        {
            prescription.Id = Guid.NewGuid();
            prescription.CreatedAt = DateTimeOffset.UtcNow;
            prescription.ModifiedAt = DateTimeOffset.UtcNow;

            await _context.Prescriptions.AddAsync(prescription);
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
            _logger.LogError(ex, "Error creating prescription.");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(Models.Prescription prescription)
    {
        if (prescription == null)
        {
            _logger.LogWarning("UpdateAsync called with null entity.");
            return false;
        }

        prescription.ModifiedAt = DateTimeOffset.UtcNow;

        try
        {
            _context.Prescriptions.Update(prescription);
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
            _logger.LogError(ex, "Error updating prescription.");
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
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription == null)
            {
                _logger.LogWarning("Prescription with ID {PrescriptionId} not found for deletion.", id);
                return false;
            }

            _context.Prescriptions.Remove(prescription);
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
            _logger.LogError(ex, "Error deleting prescription.");
            throw;
        }
    }
}
