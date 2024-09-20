using System.ComponentModel.DataAnnotations;

namespace AnnPrepareLavni.ApiService.Features.Address.Contracts;

public class AddressRequest
{
    public Guid PatientId { get; set; }
    public required string Street1 { get; set; }
    public string? Street2 { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string PostalCode { get; set; }
    public required string Country { get; set; }

}
