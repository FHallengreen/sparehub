using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.MySql;

public class AddressEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public required string AddressLine { get; init; }
    public required string PostalCode { get; init; }
    public required string Country { get; init; }

    public ICollection<ContactInfoEntity> ContactInfos { get; set; } = new List<ContactInfoEntity>();
}
