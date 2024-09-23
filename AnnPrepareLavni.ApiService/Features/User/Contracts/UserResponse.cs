using AnnPrepareLavni.ApiService.Models.Enums;
using AnnPrepareLavni.ApiService.Models;

namespace AnnPrepareLavni.ApiService.Features.User.Contracts;

public class UserResponse
{
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Language Language { get; set; }
    public UserRole Role { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
