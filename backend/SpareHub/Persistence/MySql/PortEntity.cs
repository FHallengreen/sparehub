using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.MySql;

public class PortEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public string Name { get; set; } = null!;

    public ICollection<VesselAtPortEntity> VesselAtPorts { get; set; } = new List<VesselAtPortEntity>();
}
