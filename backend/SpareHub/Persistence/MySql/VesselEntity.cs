using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Domain.MySql;

namespace Persistence.MySql;

public class VesselEntity
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int Id { get; set; }
    
    public int OwnerId { get; init; } // Ensure this is set correctly when creating a VesselEntity
    
    public required string Name { get; init; }
    public string? ImoNumber { get; init; }
    public string? Flag { get; init; }
    
    // Consider making Owner nullable if you want to allow creation without it
    public required OwnerEntity Owner { get; init; } // Ensure this is set correctly

    [JsonIgnore]
    public ICollection<VesselAtPortEntity> VesselAtPorts { get; set; } = new List<VesselAtPortEntity>();
}