namespace Shared.Order

{
    public class BoxResponse
    {
        public string Id { get; set; } = null!;
        public string OrderId { get; set; } = null!;
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public double Weight { get; set; }
    }
}
