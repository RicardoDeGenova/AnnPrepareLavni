using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AnnPrepareLavni.FrontEnd.Authentication;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private string? _authToken;

    public Task MarkUserAsAuthenticated(string token)
    {
        _authToken = token;

        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "AuthenticatedUser")
        }, "apiauth");

        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));

        return Task.CompletedTask;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (string.IsNullOrEmpty(_authToken))
        {
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        }

        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "AuthenticatedUser")
        }, "apiauth");

        var user = new ClaimsPrincipal(identity);

        return Task.FromResult(new AuthenticationState(user));
    }

    public Task LogoutAsync()
    {
        _authToken = null;
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
        return Task.CompletedTask;
    }
}
