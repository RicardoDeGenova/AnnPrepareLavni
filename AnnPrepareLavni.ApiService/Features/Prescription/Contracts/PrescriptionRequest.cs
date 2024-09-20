namespace AnnPrepareLavni.ApiService.Features.Prescription.Contracts;

public class PrescriptionRequest
{
    public Guid PatientId { get; set; }
    public Guid PrescriberId { get; set; }
    public DateTime DatePrescribed { get; set; } = DateTime.Now;
    public string DosageInstructions { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int Refills { get; set; }

    public string Notes { get; set; } = string.Empty;
}
