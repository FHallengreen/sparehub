using Domain.Models;

namespace Shared.DTOs.Dispatch;

public class DispatchResponse
{
    public string Id { get; set; } = null!;
    public string OriginType { get; set; } = null!;
    public int OriginId { get; set; }
    public string DestinationType { get; set; } = null!;
    public int? DestinationId { get; set; }
    public string DispatchStatus { get; set; } = null!;
    public string TransportModeType { get; set; } = null!;
    public string? TrackingNumber { get; set; }
    public DateTime? DispatchDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}