using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.MySql;

public class UserEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required string Name { get; set; }
    public int RoleId { get; set; } 
    public RoleEntity? Role { get; set; } 
    public int OwnerId { get; set; }
    public OwnerEntity Owner { get; set; } = null!;
}