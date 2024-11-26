namespace Persistence.MySql;

public class VesselAtPortEntity
{
    public int VesselId { get; init; }
    public int PortId { get; init; }
    public DateTime? ArrivalDate { get; set; }
    public DateTime? DepartureDate { get; set; }

    public VesselEntity VesselEntity { get; set; } = null!;
    public PortEntity PortEntity { get; set; } = null!;
}
