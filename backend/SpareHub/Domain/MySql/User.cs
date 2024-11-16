using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required string Name { get; set; }
    public int RoleId { get; set; } 
    public Role? Role { get; set; } 
}
