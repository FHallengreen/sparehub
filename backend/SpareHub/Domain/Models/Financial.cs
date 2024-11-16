using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class Financial
{
    public required string Id { get; set; }
    
    public int InvoiceId { get; init; }
    public int CostTypeId { get; init; }
    public int CurrencyId { get; init; }

}
