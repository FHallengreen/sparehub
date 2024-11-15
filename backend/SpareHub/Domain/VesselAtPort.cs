namespace Domain;

public class VesselAtPort
{
    public int VesselId { get; init; }
    public int PortId { get; init; }
    public DateTime? ArrivalDate { get; set; }
    public DateTime? DepartureDate { get; set; }

    public Vessel Vessel { get; set; } = null!;
    public Port Port { get; set; } = null!;
}