namespace Shared.DTOs.Dispatch;

public class DispatchRequest
{
    public string OriginType { get; set; } = null!;
    public int OriginId { get; set; }
    public string DestinationType { get; set; } = null!;
    public int? DestinationId { get; set; }
    public string TransportModeType { get; set; } = null!;
    public int UserId { get; set; }
}