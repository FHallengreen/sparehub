using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.MySql;
public class CurrencyEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public required string Code { get; init; }
}
