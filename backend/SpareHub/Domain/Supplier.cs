using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

// Supplier
public class Supplier
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public required string Name { get; init; }
    public int AddressId { get; init; }

    public Address Address { get; set; }
}
