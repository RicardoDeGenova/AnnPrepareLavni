

namespace AnnPrepareLavni.ApiService.Models;

public enum MedicalConditionType
{
    Chronic,
    Past,
    Current
}

public class MedicalCondition 
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public MedicalConditionType ConditionType { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset StoppedAt { get; set; }

    public Guid PatientId { get; set; }
    public Patient Patient { get; set; }

    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
