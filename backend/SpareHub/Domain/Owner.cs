using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class Owner
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public required string Name { get; init; }

    public ICollection<User> Users { get; init; } = new List<User>();
    public ICollection<Vessel> Vessels { get; init; } = new List<Vessel>();
}
