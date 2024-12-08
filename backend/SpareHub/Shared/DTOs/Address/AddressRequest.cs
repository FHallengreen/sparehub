namespace Shared.DTOs.Address;

public class AddressRequest
{
    public required string AddressLine { get; set; }
    public required string PostalCode { get; set; }
    public required string Country { get; set; }
}