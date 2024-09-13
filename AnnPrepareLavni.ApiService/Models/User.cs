

using AnnPrepareLavni.ApiService.Models.Enums;

namespace AnnPrepareLavni.ApiService.Models;

public enum Role
{
    Admin,
    Operator
}

public enum Occupation
{
    Attendant,
    Nurse,
    Doctor
}

public class User 
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string UserName { get; set; }
    public Occupation Occupation { get; set; }
    public Language Language { get; set; }
    public Role Role { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
