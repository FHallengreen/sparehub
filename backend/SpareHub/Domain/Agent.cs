using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

// Agent
public class Agent
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public required string Name { get; init; }
}
