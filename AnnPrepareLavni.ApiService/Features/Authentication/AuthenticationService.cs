using AnnPrepareLavni.ApiService.Data;
using AnnPrepareLavni.ApiService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AnnPrepareLavni.ApiService.Features.Authentication;

public interface IAuthenticationService
{
    string GenerateAccessToken(Models.User user);
    string GenerateAccessToken(IEnumerable<Claim> claims, DateTime? expiration = null);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredAccessToken(string token);
    Task SaveRefreshTokenAsync(Guid userId, string refreshToken, string userAgent);
    Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken);
    Task RemoveRefreshTokenAsync(Guid userId, string refreshToken);
    Task RemoveAllRefreshTokensAsync(Guid userId);
    string HashToken(string token);
}

public class AuthenticationService : IAuthenticationService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AuthenticationService> _logger;

    private readonly byte[] _secret;
    private readonly double _accessTokenExpirationInMinutes;
    private readonly double _refreshTokenExpirationInMinutes;
    private readonly string _issuer;
    private readonly string _audience;

    public AuthenticationService(
        ApplicationDbContext context,
        ILogger<AuthenticationService> logger,
        IConfiguration configuration)
    {
        _context = context;
        _logger = logger;

        _secret = Encoding.ASCII.GetBytes(configuration["JwtSettings:SecretKey"] ?? string.Empty);
        _accessTokenExpirationInMinutes = Convert.ToDouble(configuration["JwtSettings:AccessTokenExpirationInMinutes"]);
        _refreshTokenExpirationInMinutes = Convert.ToDouble(configuration["JwtSettings:RefreshTokenExpirationInMinutes"]);
        _issuer = configuration["JwtSettings:Issuer"] ?? string.Empty;
        _audience = configuration["JwtSettings:Audience"] ?? string.Empty;
    }

    public string GenerateAccessToken(Models.User user)
    {
        var expiration = DateTime.UtcNow.AddMinutes(_accessTokenExpirationInMinutes);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}"),
            new(ClaimTypes.Role, user.Role.ToString()),
            new(ClaimTypes.Email, user.Email.ToString()),
            new(ClaimTypes.Expiration, expiration.ToString()),
        };

        return GenerateAccessToken(claims, expiration);
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims, DateTime? expiration = null)
    {
        expiration ??= DateTime.UtcNow.AddMinutes(_accessTokenExpirationInMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiration,
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredAccessToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
            ValidIssuer = _issuer,
            ValidAudience = _audience,
            IssuerSigningKey = new SymmetricSecurityKey(_secret)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }

    public async Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
    {
        if (userId == Guid.Empty)
        {
            _logger.LogWarning("userId cannot be empty.");
            return false;
        }

        var hashedToken = HashToken(refreshToken);
        var savedToken = await _context.RefreshTokens.FirstOrDefaultAsync(u => u.UserId == userId && u.Token == hashedToken);

        if (savedToken is null)
            return false;

        if (savedToken.ExpiryDateUtc < DateTime.UtcNow)
        {
            await RemoveRefreshTokenAsync(userId, refreshToken);
            return false;
        }

        return true;
    }

    public async Task SaveRefreshTokenAsync(Guid userId, string refreshToken, string deviceInfo)
    {
        if (userId == Guid.Empty || string.IsNullOrEmpty(refreshToken))
        {
            _logger.LogWarning($"{nameof(SaveRefreshTokenAsync)} called with invalid parameters.");
            return;
        }

        var hashedToken = HashToken(refreshToken);
        var existingToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.UserId == userId && rt.DeviceInfo == deviceInfo && rt.ExpiryDateUtc >= DateTime.UtcNow);

        if (existingToken is not null)
        {
            existingToken.Token = hashedToken;
            existingToken.ExpiryDateUtc = DateTime.UtcNow.AddMinutes(_refreshTokenExpirationInMinutes);
            _context.RefreshTokens.Update(existingToken);
        }
        else
        {
            existingToken = new RefreshToken
            {
                UserId = userId,
                Token = hashedToken,
                ExpiryDateUtc = DateTime.UtcNow.AddMinutes(_refreshTokenExpirationInMinutes),
                CreatedAt = DateTime.UtcNow,
                DeviceInfo = deviceInfo
            };

            await _context.RefreshTokens.AddAsync(existingToken);
        }

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
            _context.RefreshTokens.Remove(tokenToRemove);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveAllRefreshTokensAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            _logger.LogWarning($"{nameof(RemoveAllRefreshTokensAsync)} called with empty UserId.");
            return;
        }

        var tokensToRemove = await _context.RefreshTokens
            .Where(rt => rt.UserId == userId)
            .ToListAsync();

        if (tokensToRemove != null && tokensToRemove.Count > 0)
        {
            _context.RefreshTokens.RemoveRange(tokensToRemove);
            await _context.SaveChangesAsync();
        }
    }

    public string HashToken(string token)
    {
        using var hmac = new HMACSHA512(_secret);
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(hash);
    }
}
