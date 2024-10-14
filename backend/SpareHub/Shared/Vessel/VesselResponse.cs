namespace Shared;

public class VesselResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? ImoNumber { get; init; }
    public string? Flag { get; init; }
}
