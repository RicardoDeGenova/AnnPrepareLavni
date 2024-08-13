namespace AnnPrepareLavni.Domain.Abstract.Domain.Entities;

public record MedicalConditionId(Guid Value);

public interface IMedicalCondition
{
    MedicalConditionId Id { get; set; }
    string Name { get; set; }
    string Description { get; set; }
    DateTime DateOfReport { get; set; }
}
