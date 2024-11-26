using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.MySql;

public class FinancialEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    
    public int InvoiceId { get; init; }
    public int CostTypeId { get; init; }
    public int CurrencyId { get; init; }

    // Navigation properties
    public required InvoiceEntity InvoiceEntity { get; init; }
    public required CostTypeEntity CostTypeEntity { get; init; }
    public required CurrencyEntity CurrencyEntity { get; init; }
}
