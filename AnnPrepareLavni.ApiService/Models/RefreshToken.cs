namespace AnnPrepareLavni.ApiService.Models;

public class RefreshToken
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRevoked { get; set; }
    public string ReplacedByToken { get; set; } = string.Empty;
    public string DeviceInfo { get; set; } = string.Empty;
}