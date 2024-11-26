using System.ComponentModel.DataAnnotations.Schema;
using Domain.MySql;

namespace Persistence.MySql;
public class RoleEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public required string Title { get; init; }
    public ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
}
