using System.Text.Json.Serialization;

namespace Persistence.Neo4j;

public class OwnerNode
{
    public int Id { get; init; }
    public required string Name { get; init; }


    [JsonIgnore]
    public ICollection<VesselNode> Vessels { get; init; } = new List<VesselNode>();
}