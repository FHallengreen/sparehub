using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.MySql;

public class OperatorEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int Id { get; set; }

    [Required]
    [MaxLength(45)]
    public required string Name { get; set; }

    [Required]
    [MaxLength(45)]
    public required string Title { get; set; }

    public int UserId { get; set; }

    public UserEntity User { get; set; } = null!;
}
