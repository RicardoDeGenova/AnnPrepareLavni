using AnnPrepareLavni.Domain.Abstract.Domain.Entities;

namespace AnnPrepareLavni.Domain.Implementation.Entities;

public class Address : IAddress
{
    public required AddressId Id { get; init; }
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}