
namespace Domain.Models;

public class Vessel
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string ImoNumber { get; set; } = null!;
    public string Flag { get; set; } = null!;
    public Owner Owner { get; set; } = null!;
}