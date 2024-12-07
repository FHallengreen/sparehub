namespace Shared.DTOs.Order;

public class TrackingResponse
{
    public string StatusDescription { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string Timestamp { get; set; } = null!;
    public string EstimatedDelivery { get; set; } = null!;
}