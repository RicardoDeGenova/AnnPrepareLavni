using AnnPrepareLavni.ApiService.Features.Prescription.Contracts;
using AnnPrepareLavni.ApiService.Models.Enums;

namespace AnnPrepareLavni.ApiService.Features.Medication.Contracts;

public class MedicationResponse
{
    public string Name { get; set; } = string.Empty;
    public DosageForm DosageForm { get; set; }
    public StrengthUnit StrengthUnit { get; set; }
    public double Strength { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;

    public ICollection<PrescriptionResponse> Prescriptions { get; set; } = [];
}
