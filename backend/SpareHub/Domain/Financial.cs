using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class Financial
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public int InvoiceId { get; init; }
    public int CostTypeId { get; init; }
    public int CurrencyId { get; init; }

    public Invoice Invoice { get; set; } 
    public CostType CostType { get; set; } 
    public Currency Currency { get; set; } 
}
