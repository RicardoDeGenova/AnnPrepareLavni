﻿namespace AnnPrepareLavni.ApiService.Models;

public class Exam 
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid ProviderId { get; set; }
    public string Pulmonary { get; set; } = string.Empty;
    public string Neurological { get; set; } = string.Empty;
    public string General { get; set; } = string.Empty;
    public string Cardiovascular { get; set; } = string.Empty;
    public string AbdominalGenitourinary { get; set; } = string.Empty;
    public string Extremities { get; set; } = string.Empty;
    public string Evaluation { get; set; } = string.Empty;
    public DateTime ExaminedAt { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
