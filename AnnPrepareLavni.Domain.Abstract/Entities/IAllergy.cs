namespace AnnPrepareLavni.Domain.Abstract.Domain.Entities;

public record AllegyId(Guid Value);

public interface IAllergy
{
    AllegyId Id { get; set; }
    string Name { get; set; }
}
