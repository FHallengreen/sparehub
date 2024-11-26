using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Persistence.MySql;

public class VesselEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public int OwnerId { get; init; }
    public required string Name { get; init; }
    public string? ImoNumber { get; init; }
    public string? Flag { get; init; }
    public required OwnerEntity Owner { get; init; }

    [JsonIgnore]
    public ICollection<VesselAtPortEntity> VesselAtPorts { get; set; } = new List<VesselAtPortEntity>();
}
