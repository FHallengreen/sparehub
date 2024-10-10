using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class Owner
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public required string Name { get; init; }

    public ICollection<User> Users { get; set; } = new List<User>();
}
