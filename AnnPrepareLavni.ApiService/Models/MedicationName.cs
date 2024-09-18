
using AnnPrepareLavni.ApiService.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace AnnPrepareLavni.ApiService.Models;

public class MedicationName 
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Language Language { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public ICollection<Medication>? Medications { get; set; }
}