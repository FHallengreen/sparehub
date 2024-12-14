using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Persistence.MySql;

namespace Persistence.MySql;

public class PortEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int Id { get; init; }
    public string Name { get; set; } = null!;

    [JsonIgnore]
    public ICollection<VesselAtPortEntity> VesselAtPorts { get; set; } = new List<VesselAtPortEntity>();
}
