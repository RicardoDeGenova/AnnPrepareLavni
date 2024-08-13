using AnnPrepareLavni.Domain.Abstract.Enums;

namespace AnnPrepareLavni.Domain.Abstract.Domain.Entities;

public record MedicationId(Guid Value);

public interface IMedication
{
    MedicationId Id { get; set; }
    string Name { get; set; }
    decimal Dosage { get; set; }
    DosageType DosageType { get; set; }
}
