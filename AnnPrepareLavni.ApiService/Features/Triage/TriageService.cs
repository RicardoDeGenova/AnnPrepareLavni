using AnnPrepareLavni.ApiService.Data;
using AnnPrepareLavni.ApiService.Infrastructure;
using AnnPrepareLavni.ApiService.Models;
using Microsoft.EntityFrameworkCore;

namespace AnnPrepareLavni.ApiService.Features.Triage;

public interface ITriageService : IGenericService<Models.Triage> { }

public class TriageService : ITriageService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TriageService> _logger;

    public TriageService(ApplicationDbContext context, ILogger<TriageService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Models.Triage?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            _logger.LogWarning($"{nameof(GetByIdAsync)} called with empty GUID.");
            return null;
        }

        try
        {
            return await _context.Triages
                .Include(t => t.Patient)
                .Include(t => t.Nurse)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving triage with ID {TriageId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Models.Triage>> GetAllAsync()
    {
        try
        {
            return await _context.Triages
                .Include(t => t.Patient)
                .Include(t => t.Nurse)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all triages.");
            throw;
        }
    }

    public async Task<bool> CreateAsync(Models.Triage triage)
    {
        if (triage == null)
        {
            _logger.LogWarning("CreateAsync called with null entity.");
            return false;
        }

        try
        {
            var nurse = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == triage.NurseId && u.Role == UserRole.Nurse);

            if (nurse == null)
            {
                _logger.LogWarning("CreateAsync called with invalid NurseId {NurseId}.", triage.NurseId);
                return false;
            }

            triage.Id = Guid.NewGuid();
            triage.CreatedAt = DateTimeOffset.UtcNow;
            triage.ModifiedAt = DateTimeOffset.UtcNow;

            await _context.Triages.AddAsync(triage);
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
            _logger.LogError(ex, "Error creating triage.");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(Models.Triage triage)
    {
        if (triage == null)
        {
            _logger.LogWarning("UpdateAsync called with null entity.");
            return false;
        }

        triage.ModifiedAt = DateTimeOffset.UtcNow;

        try
        {
            var nurse = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == triage.NurseId && u.Role == UserRole.Nurse);

            if (nurse == null)
            {
                _logger.LogWarning("UpdateAsync called with invalid NurseId {NurseId}.", triage.NurseId);
                return false;
            }

            _context.Triages.Update(triage);
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
            _logger.LogError(ex, "Error updating triage.");
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
            var triage = await _context.Triages.FindAsync(id);
            if (triage == null)
            {
                _logger.LogWarning("Triage with ID {TriageId} not found for deletion.", id);
                return false;
            }

            _context.Triages.Remove(triage);
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
            _logger.LogError(ex, "Error deleting triage.");
            throw;
        }
    }
}
