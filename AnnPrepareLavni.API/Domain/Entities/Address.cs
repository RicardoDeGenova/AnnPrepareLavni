namespace AnnPrepareLavni.API.Domain.Entities;

public class Address
{
    public required Guid Id { get; init; }
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}