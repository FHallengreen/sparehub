using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain;

public class Owner
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public required string Name { get; init; }

    [JsonIgnore]
    public ICollection<User> Users { get; init; } = new List<User>();
    [JsonIgnore]
    public ICollection<Vessel> Vessels { get; init; } = new List<Vessel>();
}
