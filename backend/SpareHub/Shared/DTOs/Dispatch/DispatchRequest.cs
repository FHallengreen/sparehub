namespace Shared.DTOs.Dispatch;

public record DispatchRequest
{
    public string OriginType { get; set; } = null!;
    public int OriginId { get; set; }
    public string DestinationType { get; set; } = null!;
    public int? DestinationId { get; set; }
    public string TransportModeType { get; set; } = null!;
    public int UserId { get; set; }
    public List<string> OrderIds { get; set; } = new List<string>();
}
