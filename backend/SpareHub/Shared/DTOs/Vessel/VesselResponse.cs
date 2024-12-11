using Shared.DTOs.Owner;

namespace Shared.DTOs.Vessel;

public class VesselResponse
{
    public required string Id { get; set; } = null!;
    public required string Name { get; set; } 
    public required string ImoNumber { get; set; }
    public required string Flag { get; set; }
    public OwnerResponse Owner { get; set; } = null!;
}
