namespace AnnPrepareLavni.ApiService.Models;

public enum MedicalConditionType
{
    Chronic = 1,
    Past = 2,
    Current = 3
}

public class MedicalCondition 
{
    public Guid Id { get; set; }

    public Guid? PatientId { get; set; }
    public Patient? Patient { get; set; } = null!;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public MedicalConditionType ConditionType { get; set; }

    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset StoppedAt { get; set; }

    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
}
