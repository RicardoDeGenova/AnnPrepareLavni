using AnnPrepareLavni.ApiService.Models;

namespace AnnPrepareLavni.ApiService.Features.MedicalCondition.Contracts;

public class MedicalConditionResponse
{
    public Guid PatientId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public MedicalConditionType ConditionType { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset StoppedAt { get; set; }

    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
