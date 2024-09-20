namespace AnnPrepareLavni.ApiService.Models;

public class Visit
{
    public Guid Id { get; set; }

    public Guid PatientId { get; set; }
    public Patient Patient { get; set; } = null!;

    public DateTime AdmittedAt { get; set; }
    public DateTime? DischargedAt { get; set; }

    public Guid? AssignedDoctorId { get; set; }
    public User? AssignedDoctor { get; set; }

    public string Notes { get; set; } = string.Empty;

    public ICollection<Triage> Triages { get; set; } = new List<Triage>();
    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
