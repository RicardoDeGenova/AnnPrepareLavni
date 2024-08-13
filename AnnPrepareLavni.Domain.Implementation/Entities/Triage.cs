using AnnPrepareLavni.Domain.Abstract.Domain.Entities;

namespace AnnPrepareLavni.Domain.Implementation.Entities;

public class Triage : ITriage
{
    public TriageId Id { get; set; }
    public IPatient Patient { get; set; }
    public DateTime Date { get; set; }
    public string Complaint { get; set; }
    public double Temperature { get; set; }
    public double BloodPressure { get; set; }
    public double OxygenSaturation { get; set; }
    public double SugarLevel { get; set; }
    public string ChronicConditions { get; set; }
    public string Medications { get; set; }
}
