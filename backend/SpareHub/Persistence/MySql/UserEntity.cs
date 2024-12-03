using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Persistence.MySql;

public class UserEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(45)]
    public required string Name { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Email { get; set; }

    [Required]
    [MaxLength(45)]
    public required string Password { get; set; }

    [Column("role_id")]
    public int RoleId { get; set; }

    public RoleEntity Role { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    [JsonIgnore]
    public ICollection<OwnerEntity> Owners { get; set; } = new List<OwnerEntity>();

    [JsonIgnore]
    public OperatorEntity? Operator { get; set; }
}
