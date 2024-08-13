namespace AnnPrepareLavni.Domain.Abstract.Domain.Entities;

public record ExamId(Guid Value);

public interface IExam
{
    ExamId Id { get; set; }
    Guid PatientId { get; set; }
    string General { get; set; }
    string Pulmonary { get; set; }
    string Neurological { get; set; }
    string Cardiovascular { get; set; }
    string Abdominal_Genitourinary { get; set; }
    string Extremities { get; set; }
    string Evaluation { get; set; }
    IProvider Provider { get; set; }
    DateTime ExaminedAt { get; set; }
}
