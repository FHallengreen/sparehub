using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class Port
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public string Name { get; set; } = null!;

    public ICollection<VesselAtPort> VesselAtPorts { get; set; } = new List<VesselAtPort>();
}