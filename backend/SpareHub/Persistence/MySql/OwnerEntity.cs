using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Persistence.MySql;

namespace Domain.MySql;

public class OwnerEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public required string Name { get; init; }

    [JsonIgnore]
    public ICollection<UserEntity> Users { get; init; } = new List<UserEntity>();
    [JsonIgnore]
    public ICollection<VesselEntity> Vessels { get; init; } = new List<VesselEntity>();
}
