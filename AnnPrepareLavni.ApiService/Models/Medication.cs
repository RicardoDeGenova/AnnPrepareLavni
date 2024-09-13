
using AnnPrepareLavni.ApiService.Models.Enums;

namespace AnnPrepareLavni.ApiService.Models;

public class Medication 
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Language Language { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
