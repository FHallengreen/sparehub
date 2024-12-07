using Shared.DTOs.Vessel;

namespace Shared.DTOs.VesselAtPort;

public class VesselAtPortResponse
{
    public required string PortId { get; set; } = null!;

    public List<VesselResponse> Vessels { get; set; } = [];
}