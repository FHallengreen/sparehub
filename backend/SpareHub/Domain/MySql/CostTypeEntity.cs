using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.MySql;

public class CostTypeEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public required string Type { get; init; }
}
