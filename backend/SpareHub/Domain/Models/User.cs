using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class User
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string? Role { get; set; }
    public Operator? Operator { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
