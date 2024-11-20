namespace Shared.DTOs.Vessel;

public class VesselAtPortDto
{
    public required string PortName { get; set; }

    public List<VesselResponse> Vessels { get; set; } = new();
}
