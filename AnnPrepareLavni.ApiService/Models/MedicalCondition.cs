using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AnnPrepareLavni.ApiService.Models;

public enum MedicalConditionType
{
    Chronic,
    Past,
    Current
}

public class MedicalCondition 
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid PatientId { get; set; }
    public Patient Patient { get; set; } = null!;

    [Required(ErrorMessage = "Condition name is required.")]
    [StringLength(150, ErrorMessage = "Condition name cannot be longer than 150 characters.")]
    public required string Name { get; set; }

    [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
    public string? Description { get; set; }

    [DefaultValue(MedicalConditionType.Current)]
    public MedicalConditionType ConditionType { get; set; }

    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset StoppedAt { get; set; }

    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
