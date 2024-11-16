using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class User
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public int RoleId { get; set; } 
    public Role? Role { get; set; } 
}
