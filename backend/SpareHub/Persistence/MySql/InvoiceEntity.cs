using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.MySql;

public class InvoiceEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    public int DispatchId { get; set; } 

    public DispatchEntity dispatchEntity { get; set; } = null!;
}
