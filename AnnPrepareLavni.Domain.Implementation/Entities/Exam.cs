using AnnPrepareLavni.Domain.Abstract.Domain.Entities;

namespace AnnPrepareLavni.Domain.Implementation.Entities;

public class Exam : IExam
{
    public ExamId Id { get; set; }
    public Guid PatientId { get; set; }
    public string General { get; set; }
    public string Pulmonary { get; set; }
    public string Neurological { get; set; }
    public string Cardiovascular { get; set; }
    public string Abdominal_Genitourinary { get; set; }
    public string Extremities { get; set; }
    public string Evaluation { get; set; }
    public IProvider Provider { get; set; }
    public DateTime ExaminedAt { get; set; }
}
