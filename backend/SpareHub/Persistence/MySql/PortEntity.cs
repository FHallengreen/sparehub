using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Persistence.MySql;

namespace Persistence.MySql;

public class PortEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<VesselAtPortEntity> VesselAtPorts { get; set; } = new List<VesselAtPortEntity>();
}
