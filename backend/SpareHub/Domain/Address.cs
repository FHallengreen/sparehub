using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class Address
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public required string AddressLine { get; init; }
    public required string PostalCode { get; init; }
    public required string Country { get; init; }

    public ICollection<ContactInfo> ContactInfos { get; set; } = new List<ContactInfo>();
}
