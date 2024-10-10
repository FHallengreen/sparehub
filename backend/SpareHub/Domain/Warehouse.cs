using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class Warehouse
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public required string Name { get; init; }
    public int AgentId { get; init; }

    public Agent Agent { get; set; } 
}
