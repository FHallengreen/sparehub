namespace Shared.Order;

public class BoxRequest
{
    public Guid BoxId { get; set; } // Change from int to Guid
    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public double Weight { get; set; }
}
