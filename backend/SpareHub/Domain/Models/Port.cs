using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class Port
{
    public required string Id { get; set; }
    public string Name { get; set; } = null!;

}
