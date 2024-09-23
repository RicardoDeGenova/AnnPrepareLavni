using AnnPrepareLavni.ApiService.Features.Authentication.Contracts;
using AnnPrepareLavni.ApiService.Features.User;
using AnnPrepareLavni.ApiService.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AnnPrepareLavni.ApiService.Features.Authentication;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(
        IUserService userService, 
        IAuthenticationService authenticationService)
    {
        _userService = userService;
        _authenticationService = authenticationService;
    }

    [HttpPost("Login"), AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] AuthenticationRequest request)
    {
        var user = await _userService.AuthenticateAsync(request.Username, request.Password);
        
        if (user == null)
            return Unauthorized(new ReturnMessage("Username or password is incorrect"));

        var accessToken = _authenticationService.GenerateAccessToken(user);
        var refreshToken = _authenticationService.GenerateRefreshToken();
        var deviceInfo = Request?.Headers?.UserAgent.ToString() ?? string.Empty;
        
        await _authenticationService.SaveRefreshTokenAsync(user.Id, refreshToken, deviceInfo);

        return Ok(new AuthenticationResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        });
    }

    [HttpPost("Refresh"), AllowAnonymous]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        if (request is null)
        {
            return BadRequest(new ReturnMessage("Invalid client request"));
        }

        ClaimsPrincipal? principal;
        try
        {
            principal = _authenticationService.GetPrincipalFromExpiredAccessToken(request.AccessToken);
        }
        catch
        {
            return Unauthorized(new ReturnMessage("Invalid access token"));
        }

        var success = Guid.TryParse(principal.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);
        if (!success || principal is null)
        {
            return Unauthorized(new ReturnMessage("Invalid access token"));
        }

        var isTokenValid = await _authenticationService.ValidateRefreshTokenAsync(userId, request.RefreshToken);
        if (!isTokenValid)
        {
            return Unauthorized(new ReturnMessage("Invalid or expired refresh token"));
        }

        var newAccessToken = _authenticationService.GenerateAccessToken(principal.Claims);
        var newRefreshToken = _authenticationService.GenerateRefreshToken();
        var deviceInfo = Request?.Headers?.UserAgent.ToString() ?? string.Empty;

        await _authenticationService.RemoveRefreshTokenAsync(userId, request.RefreshToken);
        await _authenticationService.SaveRefreshTokenAsync(userId, newRefreshToken, deviceInfo);

        return Ok(new AuthenticationResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        });
    }

    [Authorize]
    [HttpPost("Logout")]
    public async Task<IActionResult> Logout([FromBody] string refreshToken)
    {
        var success = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);
        if (!success)
        {
            return Unauthorized(new ReturnMessage("Invalid access token"));
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