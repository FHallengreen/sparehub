using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class Financial
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    
    public int InvoiceId { get; init; }
    public int CostTypeId { get; init; }
    public int CurrencyId { get; init; }

    // Navigation properties
    public required Invoice Invoice { get; init; } 
    public required CostType CostType { get; init; } 
    public required Currency Currency { get; init; } 
}
