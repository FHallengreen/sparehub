using System.Text.Json.Serialization;

namespace Persistence.Neo4j;

public class VesselAtPortRelationship
{
    public required int VesselId { get; init; } 
    public required int PortId { get; init; }
    public DateTime? ArrivalDate { get; set; }
    public DateTime? DepartureDate { get; set; }
    
    [JsonIgnore]
    public VesselNode VesselNode { get; set; } = null!;
    public PortNode PortNode { get; set; } = null!;
}