namespace AnnPrepareLavni.ApiService.Models;

public class Department
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public ICollection<User> Staff { get; set; } = new List<User>();
}
