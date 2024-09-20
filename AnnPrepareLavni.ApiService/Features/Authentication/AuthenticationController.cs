using AnnPrepareLavni.ApiService.Features.Authentication.Contracts;
using AnnPrepareLavni.ApiService.Features.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AnnPrepareLavni.ApiService.Features.Authentication;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtService _jwtService;
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(
        IUserService userService, 
        IJwtService jwtService,
        IAuthenticationService authenticationService)
    {
        _userService = userService;
        _jwtService = jwtService;
        _authenticationService = authenticationService;
    }

    [HttpPost("login"), AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] AuthenticationRequest request)
    {
        var user = await _userService.AuthenticateAsync(request.Username, request.Password);

        if (user == null)
            return Unauthorized(new { Message = "Username or password is incorrect" });

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };

        var accessToken = _jwtService.GenerateAccessToken(claims);
        var refreshToken = _jwtService.GenerateRefreshToken();

        await _authenticationService.SaveRefreshTokenAsync(user.Id, refreshToken, DateTime.UtcNow.AddDays(7));

        return Ok(new AuthenticationResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        if (request is null)
        {
            return BadRequest("Invalid client request");
        }

        string accessToken = request.AccessToken;
        string refreshToken = request.RefreshToken;

        var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken);
        var success = Guid.TryParse(principal.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);
        if (!success)
        {
            return Unauthorized("Invalid access token");
        }

        var isTokenValid = await _authenticationService.ValidateRefreshTokenAsync(userId, refreshToken);
        if (!isTokenValid)
        {
            return Unauthorized("Invalid or expired refresh token");
        }

        var newAccessToken = _jwtService.GenerateAccessToken(principal.Claims);
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        await _authenticationService.SaveRefreshTokenAsync(userId, newRefreshToken, DateTime.UtcNow.AddDays(7), refreshToken);

        return Ok(new AuthenticationResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] string refreshToken)
    {
        var success = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);
        if (!success)
        {
            return Unauthorized("Invalid access token");
        }

        if (string.IsNullOrEmpty(refreshToken))
        {
            await _authenticationService.RemoveAllRefreshTokensAsync(userId);
        }
        else
        {
            await _authenticationService.RemoveRefreshTokenAsync(userId, refreshToken);
        }

        return NoContent();
    }
}