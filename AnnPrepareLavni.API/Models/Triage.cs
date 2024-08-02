namespace AnnPrepareLavni.API;

public class Triage
{
    public int Id { get; set; }
    public int PatientNumber { get; set; }
    public Guid PatientId { get; set; }
    public DateTime Date { get; set; }
    public string Complaint { get; set; }
    public double Temperature { get; set; }
    public double BloodPressure { get; set; }
    public double OxygenSaturation { get; set; }
    public double SugarLevel { get; set; }
    public string ChronicConditions { get; set; }
    public string Medications { get; set; }
}
