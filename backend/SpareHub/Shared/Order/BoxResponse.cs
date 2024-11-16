namespace Shared.Order;

public class BoxResponse
{
    public Guid Id { get; set; }
    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public double Weight { get; set; }
}
