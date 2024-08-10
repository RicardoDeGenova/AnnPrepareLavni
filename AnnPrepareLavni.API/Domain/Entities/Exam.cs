namespace AnnPrepareLavni.API.Domain.Entities;

public class Exam
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string General { get; set; }
    public string Pulmonary { get; set; }
    public string Neurological { get; set; }
    public string Cardiovascular { get; set; }
    public string Abdominal_Genitourinary { get; set; }
    public string Extremities { get; set; }
    public string Evaluation { get; set; }
    public Provider Provider { get; set; }
    public DateTime ExaminedAt { get; set; }
}
