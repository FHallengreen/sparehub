using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class Invoice
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public int DispatchId { get; init; }

    public Dispatch Dispatch { get; set; }
}
