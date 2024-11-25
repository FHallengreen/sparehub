namespace Shared.DTOs.Vessel;

public class VesselAtPortRequest
{
    public required string PortName { get; set; }

    public List<VesselResponse> Vessels { get; set; } = new();
}
