

using System.Text.Json.Serialization;

namespace Persistence.MySql;

public class VesselAtPortEntity
{
    public required int VesselId { get; init; } 
    public required int PortId { get; init; }
    public DateTime? ArrivalDate { get; set; }
    public DateTime? DepartureDate { get; set; }

    [JsonIgnore]
    public VesselEntity VesselEntity { get; set; } = null!;
    public PortEntity PortEntity { get; set; } = null!;
}
