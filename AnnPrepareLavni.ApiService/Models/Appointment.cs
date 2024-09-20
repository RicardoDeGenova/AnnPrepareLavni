namespace AnnPrepareLavni.ApiService.Models;

public enum AppointmentStatus
{
    Scheduled,
    Completed,
    Cancelled,
    NoShow
}

public class Appointment
{
    public Guid Id { get; set; }

    public Guid PatientId { get; set; }
    public Patient Patient { get; set; } = null!;

    public Guid DoctorId { get; set; }
    public User Doctor { get; set; } = null!;

    public DateTime ScheduledAt { get; set; }
    public AppointmentStatus Status { get; set; }
    public string Reason { get; set; } = string.Empty;

    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
