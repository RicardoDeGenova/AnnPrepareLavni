namespace AnnPrepareLavni.ApiService.Models;

public class RefreshToken
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiryDateUtc { get; set; }
    public string DeviceInfo { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}