namespace AnnPrepareLavni.Domain.Abstract.Domain.Entities;

public record AddressId(Guid Value);

public interface IAddress
{
    AddressId Id { get; init; }
    string Street { get; set; } 
    string City { get; set; } 
    string Country { get; set; }
}