using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Persistence.MySql;

public class VesselEntity
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int Id { get; init; }
    
    public int OwnerId { get; set; }
    
    public required string Name { get; set; }
    public string? ImoNumber { get; set; }
    public string? Flag { get; set; }
    public required OwnerEntity Owner { get; init; }

    [JsonIgnore]
    public ICollection<VesselAtPortEntity> VesselAtPorts { get; set; } = new List<VesselAtPortEntity>();
}