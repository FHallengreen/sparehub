namespace Domain.Models;

public class VesselAtPort
{
    public required string VesselId { get; init; }
    public required string PortId { get; init; }
    public DateTime? ArrivalDate { get; set; }
    public DateTime? DepartureDate { get; set; }

}
