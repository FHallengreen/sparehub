namespace Domain.Models;

public class Address
{
    public string Id { get; set; } = null!;
    public required string AddressLine { get; set; }
    public required string PostalCode { get; set; }
    public required string Country { get; set; }
}
