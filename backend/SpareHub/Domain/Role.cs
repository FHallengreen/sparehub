using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class Role
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public required string Title { get; init; }
}
