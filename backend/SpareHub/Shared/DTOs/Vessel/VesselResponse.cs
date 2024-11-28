using Shared.DTOs.Owner;

namespace Shared.DTOs.Vessel;

public class VesselResponse
{
    public required string Id { get; set; } = null!;
    public required string Name { get; set; } 
    public string? ImoNumber { get; set; }
    public string? Flag { get; set; }
    public OwnerResponse Owner { get; set; } = null!;
}
