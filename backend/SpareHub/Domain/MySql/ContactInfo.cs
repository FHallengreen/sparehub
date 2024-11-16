using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class ContactInfo
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Value { get; init; }
    public string? ContactType { get; init; }

    public ICollection<Address> Addresses { get; set; } = new List<Address>();
}
