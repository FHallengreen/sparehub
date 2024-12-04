using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.MySql;
public class ContactInfoEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Value { get; init; }
    public string? ContactType { get; init; }

    public ICollection<AddressEntity> Addresses { get; set; } = new List<AddressEntity>();
}
