namespace Shared.DTOs.Address;

public class AddressResponse
{
    public required string Id { get; set; }
    public required string AddressLine { get; set; }
    public required string PostalCode { get; set; }
    public required string Country { get; set; }
}