using AnnPrepareLavni.ApiService.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AnnPrepareLavni.ApiService.Features.MedicalCondition.Contracts;

public class MedicalConditionRequest
{
    [Required]
    public Guid PatientId { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    [DefaultValue(MedicalConditionType.Current)]
    public MedicalConditionType ConditionType { get; set; }

    public DateTimeOffset StartedAt { get; set; }

    public DateTimeOffset? StoppedAt { get; set; }
}
