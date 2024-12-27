using AnnPrepareLavni.FrontEnd.Models;

namespace AnnPrepareLavni.FrontEnd.Services;

public interface ILoginService
{
    Task<LoginResult> LoginAsync(LoginModel loginModel);
}

public class LoginService : ILoginService
{
    private readonly HttpClient _httpClient;

    public LoginService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LoginResult> LoginAsync(LoginModel loginModel)
    {
        var response = await _httpClient.PostAsJsonAsync("Authentication/Login", loginModel);

        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadAsStringAsync();
            return new LoginResult { IsSuccess = true, Token = token };
        }

        return new LoginResult { IsSuccess = false, ErrorMessage = "Invalid username or password." };
    }
}

public class LoginResult
{
    public bool IsSuccess { get; set; }
    public string? Token { get; set; }
    public string? ErrorMessage { get; set; }
}
