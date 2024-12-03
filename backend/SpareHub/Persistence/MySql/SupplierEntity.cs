using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.MySql;
// Supplier
public class SupplierEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public required string Name { get; init; }
    public int AddressId { get; init; }

    public required AddressEntity AddressEntity { get; init; }
}
