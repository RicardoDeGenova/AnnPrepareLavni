using AnnPrepareLavni.ApiService.Data;
using AnnPrepareLavni.ApiService.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace AnnPrepareLavni.ApiService.Features.Authentication;

public interface IAuthenticationService
{
    Task SaveRefreshTokenAsync(Guid userId, string refreshToken, DateTime expiryDate, string previousToken = null);
    Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken);
    Task RemoveRefreshTokenAsync(Guid userId, string refreshToken);
    Task RemoveAllRefreshTokensAsync(Guid userId);
}

public class AuthenticationService : IAuthenticationService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(ApplicationDbContext context, ILogger<AuthenticationService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
    {
        if (userId == Guid.Empty)
        {
            _logger.LogWarning($"{nameof(ValidateRefreshTokenAsync)} called with empty GUID.");
            return false;
        }

        var hashedToken = HashToken(refreshToken);
        var savedToken = await _context.RefreshTokens.FirstOrDefaultAsync(u => u.UserId == userId && u.Token == hashedToken && !u.IsRevoked);

        if (savedToken is null || savedToken.ExpiryDate < DateTime.UtcNow)
            return false;

        return true;        
    }

    public async Task SaveRefreshTokenAsync(Guid userId, string refreshToken, DateTime expiryDate, string previousToken = "")
    {
        if (userId == Guid.Empty || string.IsNullOrEmpty(refreshToken))
        {
            _logger.LogWarning($"{nameof(SaveRefreshTokenAsync)} called with invalid parameters.");
            return;
        }

        var hashedToken = HashToken(refreshToken);

        var newToken = new RefreshToken
        {
            UserId = userId,
            Token = hashedToken,
            CreatedAt = DateTime.UtcNow,
            ExpiryDate = expiryDate,
            IsRevoked = false,
            ReplacedByToken = string.Empty
        };

        if (!string.IsNullOrEmpty(previousToken))
        {
            var hashedPreviousToken = HashToken(previousToken);
            var existingToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == userId && rt.Token == hashedPreviousToken);

            if (existingToken != null)
            {
                existingToken.ReplacedByToken = hashedToken;
                existingToken.IsRevoked = true;
            }
        }

        await _context.RefreshTokens.AddAsync(newToken);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveRefreshTokenAsync(Guid userId, string refreshToken)
    {
        if (userId == Guid.Empty || string.IsNullOrEmpty(refreshToken))
        {
            _logger.LogWarning($"{nameof(RemoveRefreshTokenAsync)} called with invalid parameters.");
            return;
        }

        var hashedToken = HashToken(refreshToken);

        var tokenToRemove = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == userId && rt.Token == hashedToken);
        if (tokenToRemove != null)
        {
            tokenToRemove.IsRevoked = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveAllRefreshTokensAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            _logger.LogWarning($"{nameof(RemoveRefreshTokenAsync)} called with empty UserId.");
            return;
        }

        var tokensToRemove = await _context.RefreshTokens
            .Where(rt => rt.UserId == userId)
            .ToListAsync();

        if (tokensToRemove != null && tokensToRemove.Count > 0)
        {
            tokensToRemove.ForEach(x => x.IsRevoked = true);
            await _context.SaveChangesAsync();
        }
    }

    public string HashToken(string token)
    {
        var bytes = Encoding.UTF8.GetBytes(token);
        var hash = SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }
}
