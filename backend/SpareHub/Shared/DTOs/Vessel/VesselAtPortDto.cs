namespace Shared;

public class VesselAtPortDto
{
    public required string PortName { get; set; }

    public List<VesselResponse> Vessels { get; set; } = new();
}
