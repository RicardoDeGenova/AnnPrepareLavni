using AnnPrepareLavni.Domain.Abstract.Domain.Entities;

namespace AnnPrepareLavni.Domain.Implementation.Entities;

public class MedicalCondition : IMedicalCondition
{
    public MedicalConditionId Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateOfReport { get; set; }
}
