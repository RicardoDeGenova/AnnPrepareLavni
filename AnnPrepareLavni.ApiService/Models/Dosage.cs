namespace AnnPrepareLavni.ApiService.Models;

public class Dosage
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
