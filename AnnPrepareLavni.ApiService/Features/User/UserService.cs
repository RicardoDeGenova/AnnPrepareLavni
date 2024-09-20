using AnnPrepareLavni.ApiService.Data;
using AnnPrepareLavni.ApiService.Infrastructure;
using AnnPrepareLavni.ApiService.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;

namespace AnnPrepareLavni.ApiService.Features.User;

public interface IUserService : IGenericService<Models.User>
{
    Task<Models.User?> AuthenticateAsync(string username, string password);
    Task<bool> ChangePasswordAsync(Guid userId, string newPassword);
    Task<IEnumerable<Models.User>> GetUsersByRoleAsync(UserRole role);
}

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserService> _logger;

    public UserService(ApplicationDbContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Models.User?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            _logger.LogWarning($"{nameof(GetByIdAsync)} called with empty GUID.");
            return null;
        }

        try
        {
            return await _context.Users
                .Include(u => u.Prescriptions)
                .Include(u => u.Triages)
                .Include(u => u.Appointments)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with ID {UserId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Models.User>> GetAllAsync()
    {
        try
        {
            return await _context.Users
                .Include(u => u.Prescriptions)
                .Include(u => u.Triages)
                .Include(u => u.Appointments)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all users.");
            throw;
        }
    }

    public async Task<bool> CreateAsync(Models.User user)
    {
        if (user == null)
        {
            _logger.LogWarning("CreateAsync called with null entity.");
            return false;
        }

        try
        {
            user.PasswordHash = HashPassword(user.PasswordHash);

            user.Id = Guid.NewGuid();
            user.CreatedAt = DateTimeOffset.UtcNow;
            user.ModifiedAt = DateTimeOffset.UtcNow;

            await _context.Users.AddAsync(user);
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
            _logger.LogError(ex, "Error creating user.");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(Models.User user)
    {
        if (user == null)
        {
            _logger.LogWarning("UpdateAsync called with null entity.");
            return false;
        }

        user.ModifiedAt = DateTimeOffset.UtcNow;

        try
        {
            _context.Users.Update(user);
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
            _logger.LogError(ex, "Error updating user.");
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
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found for deletion.", id);
                return false;
            }

            _context.Users.Remove(user);
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
            _logger.LogError(ex, "Error deleting user.");
            throw;
        }
    }

    public async Task<Models.User?> AuthenticateAsync(string username, string password)
    {
        try
        {
            var passwordHash = HashPassword(password);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == passwordHash);

            if (user == null)
            {
                _logger.LogWarning("Authentication failed for username {Username}.", username);
            }

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during authentication for username {Username}.", username);
            throw;
        }
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, string newPassword)
    {
        if (userId == Guid.Empty)
        {
            _logger.LogWarning("ChangePasswordAsync called with empty user ID.");
            return false;
        }

        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found for password change.", userId);
                return false;
            }

            user.PasswordHash = HashPassword(newPassword);
            user.ModifiedAt = DateTimeOffset.UtcNow;

            _context.Users.Update(user);
            var updated = await _context.SaveChangesAsync();
            return updated > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password for user with ID {UserId}.", userId);
            throw;
        }
    }

    public async Task<IEnumerable<Models.User>> GetUsersByRoleAsync(UserRole role)
    {
        try
        {
            return await _context.Users
                .Where(u => u.Role == role)
                .Include(u => u.Prescriptions)
                .Include(u => u.Triages)
                .Include(u => u.Appointments)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users with role {Role}.", role);
            throw;
        }
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
