namespace AnnPrepareLavni.ApiService.Features.User.Contracts;

public class ChangePasswordRequest
{
    public Guid Id { get; set; }
    public string NewPassword { get; set; } = string.Empty;
}
