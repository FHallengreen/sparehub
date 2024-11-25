namespace Shared.DTOs.Vessel;

public class VesselRequest
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? ImoNumber { get; set; }
    public string? Flag { get; set; }
    public OwnerRequest Owner { get; set; }
}
