using AnnPrepareLavni.ApiService.Features.Patient.Contracts;
using AnnPrepareLavni.ApiService.Features.User.Contracts;

namespace AnnPrepareLavni.ApiService.Features.Prescription.Contracts;

public class PrescriptionResponse
{
    public PatientResponse Patient { get; set; } = null!;
    public UserResponse Prescriber { get; set; } = null!;
    public DateTime DatePrescribed { get; set; }
    public string DosageInstructions { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int Refills { get; set; }

    public string Notes { get; set; } = string.Empty;

    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
