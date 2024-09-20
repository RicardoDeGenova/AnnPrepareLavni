using AnnPrepareLavni.ApiService.Models.Enums;

namespace AnnPrepareLavni.ApiService.Features.Medication.Contracts;

public class MedicationRequest
{
    public string Name { get; set; } = string.Empty;
    public DosageForm DosageForm { get; set; }
    public StrengthUnit StrengthUnit { get; set; }
    public double Strength { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
}
