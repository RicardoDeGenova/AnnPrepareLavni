using AnnPrepareLavni.ApiService.Data;
using AnnPrepareLavni.ApiService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace AnnPrepareLavni.ApiService.Features.User;

public interface IUserService
{
    public Task<Models.User?> GetByIdAsync(Guid id, bool includePrescriptions = false, bool includeTriages = false, bool includeAppointments = false);
    public Task<IEnumerable<Models.User>> GetAllAsync();
    public Task<bool> CreateAsync(Models.User entity);
    public Task<bool> UpdateAsync(Models.User entity);
    public Task<bool> DeleteAsync(Guid id);
    Task<Models.User?> AuthenticateAsync(string username, string password);
    Task<bool> ChangePasswordAsync(Guid userId, string newPassword);
    Task<IEnumerable<Models.User>> GetUsersByRoleAsync(UserRole role);
    Task<Models.User?> GetByUsernameAsync(string userName);
}

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserService> _logger;
    private readonly PasswordHasher<Models.User> _passwordHasher;

    public UserService(ApplicationDbContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
        _passwordHasher = new PasswordHasher<Models.User>();
    }

    public async Task<Models.User?> GetByIdAsync(Guid id, bool includePrescriptions = false, bool includeTriages = false, bool includeAppointments = false)
    {
        if (id == Guid.Empty)
        {
            _logger.LogWarning($"{nameof(GetByIdAsync)} called with empty GUID.");
            return null;
        }

        try
        {
            var query = _context.Users.AsQueryable();

            if (includePrescriptions)
                query = query.Include(u => u.Prescriptions);

            if (includeTriages)
                query = query.Include(u => u.Triages);

            if (includeAppointments)
                query = query.Include(u => u.Appointments);

            return await query.FirstOrDefaultAsync(u => u.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with ID {UserId}", id);
            throw;
        }
    }

    public async Task<Models.User?> GetByUsernameAsync(string userName)
    {
        if (string.IsNullOrEmpty(userName))
        {
            _logger.LogWarning($"{nameof(GetByIdAsync)} called with empty userName.");
            return null;
        }

        try
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == userName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with Username {userName}", userName);
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
            user.Id = Guid.NewGuid();
            user.CreatedAt = DateTimeOffset.UtcNow;
            user.ModifiedAt = DateTimeOffset.UtcNow;

            var password = user.PasswordHash;
            user.PasswordHash = string.Empty;

            user.PasswordHash = _passwordHasher.HashPassword(user, password);

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
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
            if (user is not null)
            {
                var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
                if (result == PasswordVerificationResult.Success)
                {
                    return user;
                }
            }

            _logger.LogWarning("Authentication failed for username {Username}.", username);
            return null;
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

            user.PasswordHash = _passwordHasher.HashPassword(user, user.PasswordHash);
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
}
