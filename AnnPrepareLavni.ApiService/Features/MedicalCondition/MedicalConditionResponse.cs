using AnnPrepareLavni.ApiService.Models;

namespace AnnPrepareLavni.ApiService.Features.MedicalCondition;

public class MedicalConditionResponse
{
    public string Name { get; set; }
    public string Description { get; set; }
    public MedicalConditionType ConditionType { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset StoppedAt { get; set; }

    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
