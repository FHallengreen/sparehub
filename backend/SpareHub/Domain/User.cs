using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public required string Name { get; init; }
    public int RoleId { get; init; }

    public Role Role { get; init; } = null!;

    public ICollection<Owner> Owners { get; init; } = new List<Owner>();
}
