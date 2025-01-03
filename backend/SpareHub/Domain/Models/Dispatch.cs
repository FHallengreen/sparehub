
namespace Domain.Models;

public class Dispatch
{
    public string Id { get; set; } = null!;
    public string? OriginType { get; set; }
    public int OriginId { get; set; }
    public string? DestinationType { get; set; }
    public int? DestinationId { get; set; }
    public string DispatchStatus { get; set; } = null!;
    public string TransportModeType { get; set; } = null!;
    public string? TrackingNumber { get; set; }
    public DateTime? DispatchDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public int UserId { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
