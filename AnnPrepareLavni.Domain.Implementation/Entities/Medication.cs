using AnnPrepareLavni.Domain.Abstract.Domain.Entities;
using AnnPrepareLavni.Domain.Abstract.Enums;

namespace AnnPrepareLavni.Domain.Implementation.Entities;

public class Medication : IMedication
{
    public MedicationId Id { get; set; }
    public string Name { get; set; }
    public decimal Dosage { get; set; }
    public DosageType DosageType { get; set; }
}
