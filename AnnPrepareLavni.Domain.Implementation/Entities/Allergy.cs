using AnnPrepareLavni.Domain.Abstract.Domain.Entities;

namespace AnnPrepareLavni.Domain.Implementation.Entities;

public class Allergy : IAllergy
{
    public required AllegyId Id { get; set; }
    public required string Name { get; set; }
}
