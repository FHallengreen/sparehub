namespace Domain.Models;

public class Warehouse
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public Agent? Agent { get; set; }
}
