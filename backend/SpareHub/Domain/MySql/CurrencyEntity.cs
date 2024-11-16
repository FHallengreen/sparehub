using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.MySql;

public class CurrencyEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public required string Code { get; init; }
}
