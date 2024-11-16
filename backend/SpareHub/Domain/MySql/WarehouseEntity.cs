using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.MySql;

public class WarehouseEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public required string Name { get; init; }
    public int AgentId { get; init; }

    public required AgentEntity  Agent { get; init; } 
}
