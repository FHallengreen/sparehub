using System.Text.Json.Serialization;

namespace Persistence.Neo4j;

public class VesselNode
{
    public required int Id { get; init; }
    
    public int OwnerId { get; init; }
    
    public required string Name { get; init; }
    public string? ImoNumber { get; init; }
    public string? Flag { get; init; }
    
    public required OwnerNode Owner { get; init; }
    
    [JsonIgnore]
    public ICollection<VesselAtPortRelationship> VesselAtPorts { get; set; } = new List<VesselAtPortRelationship>();
}