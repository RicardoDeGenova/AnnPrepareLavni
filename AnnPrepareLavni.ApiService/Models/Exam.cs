namespace AnnPrepareLavni.ApiService.Models;

public class Exam 
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid ProviderId { get; set; }
    public string Pulmonary { get; set; }
    public string Neurological { get; set; }
    public string General { get; set; }
    public string Cardiovascular { get; set; }
    public string AbdominalGenitourinary { get; set; }
    public string Extremities { get; set; }
    public string Evaluation { get; set; }
    public DateTime ExaminedAt { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
