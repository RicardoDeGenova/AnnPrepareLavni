using System.ComponentModel.DataAnnotations;

namespace AnnPrepareLavni.ApiService.Features.Address.Contracts;

public class AddressRequest
{
    public Guid PatientId { get; set; }

    [Required(ErrorMessage = "Street is required.")]
    [StringLength(100, ErrorMessage = "Street1 cannot be longer than 100 characters.")]
    public required string Street1 { get; set; }

    [StringLength(100, ErrorMessage = "Street2 cannot be longer than 100 characters.")]
    public string Street2 { get; set; } = string.Empty;

    [Required(ErrorMessage = "City is required.")]
    [StringLength(50, ErrorMessage = "City cannot be longer than 50 characters.")]
    public required string City { get; set; }

    [Required(ErrorMessage = "State is required.")]
    [StringLength(50, ErrorMessage = "State cannot be longer than 50 characters.")]
    public required string State { get; set; }

    [Required(ErrorMessage = "Postal Code is required.")]
    [StringLength(10, ErrorMessage = "Postal Code cannot be longer than 10 characters.")]
    public required string PostalCode { get; set; }

    [Required(ErrorMessage = "Country is required.")]
    [StringLength(50, ErrorMessage = "Country cannot be longer than 50 characters.")]
    public required string Country { get; set; }

}
