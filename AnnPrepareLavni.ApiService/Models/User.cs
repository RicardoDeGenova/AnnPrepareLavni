

using AnnPrepareLavni.ApiService.Models.Enums;

namespace AnnPrepareLavni.ApiService.Models;

public enum UserRole
{
    Administrator,
    Doctor,
    Nurse,
    Receptionist,
    Pharmacist,
    LabTechnician
}

public class User 
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Language Language { get; set; }
    public UserRole Role { get; set; }

    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    public ICollection<Triage> Triages { get; set; } = new List<Triage>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();



    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
