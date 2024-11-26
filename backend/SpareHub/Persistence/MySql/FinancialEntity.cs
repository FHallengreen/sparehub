using System.ComponentModel.DataAnnotations.Schema;
using Domain.MySql;

namespace Persistence.MySql;
public class FinancialEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    
    public int InvoiceId { get; init; }
    public int CostTypeId { get; init; }
    public int CurrencyId { get; init; }

    // Navigation properties
    public required InvoiceEntity invoiceEntity { get; init; } 
    public required CostTypeEntity costTypeEntity { get; init; } 
    public required CurrencyEntity currencyEntity { get; init; } 
}
