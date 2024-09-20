

namespace AnnPrepareLavni.ApiService.Models;

public class Triage 
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Patient Patient { get; set; } = null!;

    public Guid NurseId { get; set; }
    public User Nurse { get; set; } = null!;

    public string Complaint { get; set; } = string.Empty;
    public double TemperatureInCelsius { get; set; }
    public string BloodPressure { get; set; } = string.Empty;
    public double OxygenSaturationLevel { get; set; }
    public string SugarLevel { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;

    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}