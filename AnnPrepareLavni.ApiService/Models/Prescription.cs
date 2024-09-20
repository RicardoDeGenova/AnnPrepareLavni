namespace AnnPrepareLavni.ApiService.Models;

public class Prescription
{
    public Guid Id { get; set; }

    public Guid PatientId { get; set; }
    public Patient Patient { get; set; } = null!;

    public Guid PrescriberId { get; set; }
    public User Prescriber { get; set; } = null!;

    public DateTime DatePrescribed { get; set; } 
    public string DosageInstructions { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int Refills { get; set; }

    public string Notes { get; set; } = string.Empty;

    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}