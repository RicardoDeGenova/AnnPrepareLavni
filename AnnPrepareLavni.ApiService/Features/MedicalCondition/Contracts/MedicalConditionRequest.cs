using AnnPrepareLavni.ApiService.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AnnPrepareLavni.ApiService.Features.MedicalCondition.Contracts;

public class MedicalConditionRequest
{
    [Required]
    public Guid PatientId { get; set; }

    [Required(ErrorMessage = "Condition name is required.")]
    [StringLength(150, ErrorMessage = "Condition name cannot be longer than 150 characters.")]
    public string Name { get; set; }

    [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
    public string Description { get; set; }

    [DefaultValue(MedicalConditionType.Current)]
    public MedicalConditionType ConditionType { get; set; }

    public DateTimeOffset StartedAt { get; set; }

    public DateTimeOffset StoppedAt { get; set; }
}
