namespace Shared.DTOs.Vessel;

public class VesselAtPortRequest
{
    public required string PortId { get; set; }

    public List<VesselResponse> Vessels { get; set; } = [];
}
