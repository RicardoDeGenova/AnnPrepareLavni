

namespace AnnPrepareLavni.ApiService.Models;

public class Triage 
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string Complaint { get; set; }
    public double Temperature { get; set; }
    public double BloodPressure { get; set; }
    public double OxygenSaturationLevel { get; set; }
    public double SugarLevel { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
