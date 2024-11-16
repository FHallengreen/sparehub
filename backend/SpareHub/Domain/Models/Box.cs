namespace Domain.Models;

public class Box
{
    public required string Id { get; set; }
    public string OrderId { get; set; } = null!;
    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public double Weight { get; set; }
}
