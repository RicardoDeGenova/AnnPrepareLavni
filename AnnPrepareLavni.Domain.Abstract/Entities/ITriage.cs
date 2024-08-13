namespace AnnPrepareLavni.Domain.Abstract.Domain.Entities;

public record TriageId(Guid Value);

public interface ITriage
{
    TriageId Id { get; set; }
    IPatient Patient { get; set; }
    DateTime Date { get; set; }
    string Complaint { get; set; }
    double Temperature { get; set; }
    double BloodPressure { get; set; }
    double OxygenSaturation { get; set; }
    double SugarLevel { get; set; }
    string ChronicConditions { get; set; }
    string Medications { get; set; }
}
