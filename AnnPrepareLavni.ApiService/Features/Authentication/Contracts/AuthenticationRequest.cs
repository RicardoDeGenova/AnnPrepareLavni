namespace AnnPrepareLavni.ApiService.Features.Authentication.Contracts;

public class AuthenticationRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
