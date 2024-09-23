namespace AnnPrepareLavni.ApiService.Features.Triage.Contracts;

public class TriageResponse
{
    public Guid PatientId { get; set; }
    public Guid NurseId { get; set; }

    public string Complaint { get; set; } = string.Empty;
    public double TemperatureInCelsius { get; set; }
    public string BloodPressure { get; set; } = string.Empty;
    public double OxygenSaturationLevel { get; set; }
    public string SugarLevel { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;

    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
