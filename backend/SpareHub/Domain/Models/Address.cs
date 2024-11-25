namespace Domain.Models;

public class Address
{
    public string Id { get; init; } = null!;
    public required string AddressLine { get; init; }
    public required string PostalCode { get; init; }
    public required string Country { get; init; }
}
