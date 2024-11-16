using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class CostType
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public required string Type { get; init; }
}
