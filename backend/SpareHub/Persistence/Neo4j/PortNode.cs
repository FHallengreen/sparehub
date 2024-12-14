

using System.Text.Json.Serialization;

namespace Persistence.Neo4j;

public class PortNode
{
    public required int Id { get; init; }
    public string Name { get; set; } = null!;
    
    
    [JsonIgnore]
    public ICollection<VesselAtPortRelationship> VesselAtPorts { get; set; } = new List<VesselAtPortRelationship>();
}