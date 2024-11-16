namespace Domain.Models;

public class ContactInfo
{
    public required string Id { get; init; }
    public string? Name { get; init; }
    public string? Value { get; init; }
    public string? ContactType { get; init; }
}
