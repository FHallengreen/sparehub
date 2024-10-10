using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class Currency
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public required string Code { get; init; }
}
