using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class Vessel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public int OwnerId { get; init; }
    public required string Name { get; init; }
    public string? ImoNumber { get; init; }
    public string? Flag { get; init; }

    public required Owner Owner { get; init; }
    
}
