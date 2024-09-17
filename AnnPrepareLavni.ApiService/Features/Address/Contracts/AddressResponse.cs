namespace AnnPrepareLavni.ApiService.Features.Address.Contracts;

public class AddressResponse
{
    public Guid Id { get; set; }
    public required string Street1 { get; set; }
    public string Street2 { get; set; } = string.Empty;
    public required string City { get; set; }
    public required string State { get; set; }
    public required string PostalCode { get; set; }
    public required string Country { get; set; }

    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
