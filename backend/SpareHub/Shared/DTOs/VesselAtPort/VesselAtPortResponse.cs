using Shared.DTOs.Vessel;

namespace Shared.DTOs.VesselAtPort;

public class VesselAtPortResponse
{
    public required string PortId { get; set; } = null!;

    public required string PortName { get; set; } = null!;

    public List<VesselResponse> Vessels { get; set; } = [];
    
    public required string ArrivalDate { get; set; }
    
    public required string DepartureDate { get; set; }
}