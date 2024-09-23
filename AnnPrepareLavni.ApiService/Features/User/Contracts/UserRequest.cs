using AnnPrepareLavni.ApiService.Models.Enums;
using AnnPrepareLavni.ApiService.Models;

namespace AnnPrepareLavni.ApiService.Features.User.Contracts;

public class UserRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Language Language { get; set; }
    public UserRole Role { get; set; }
}
