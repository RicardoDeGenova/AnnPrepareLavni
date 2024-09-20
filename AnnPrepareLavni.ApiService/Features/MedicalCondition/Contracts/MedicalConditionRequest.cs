using AnnPrepareLavni.ApiService.Models;
using System.ComponentModel;

namespace AnnPrepareLavni.ApiService.Features.MedicalCondition.Contracts;

public class MedicalConditionRequest
{
    public Guid PatientId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    [DefaultValue(MedicalConditionType.Current)]
    public MedicalConditionType ConditionType { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset? StoppedAt { get; set; }
}
