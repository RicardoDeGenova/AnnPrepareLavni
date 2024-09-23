namespace AnnPrepareLavni.ApiService.Features.Prescription.Contracts;

public class PrescriptionResponse
{
    public Guid PatientId { get; set; }
    public Guid PrescriberId { get; set; }
    public DateTime DatePrescribed { get; set; }
    public string DosageInstructions { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int Refills { get; set; }

    public string Notes { get; set; } = string.Empty;

    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
