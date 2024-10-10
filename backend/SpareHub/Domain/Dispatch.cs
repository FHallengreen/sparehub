using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class Dispatch
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public required string OriginType { get; init; } 
    public int OriginId { get; init; }
    public required string DestinationType { get; init; }
    public int? DestinationId { get; init; }
    public required string DispatchStatus { get; init; } 
    public required string TransportModeType { get; init; }
    public string? TrackingNumber { get; init; }
    public DateTime? DispatchDate { get; init; }
    public DateTime? DeliveryDate { get; init; }
    public int UserId { get; init; }

    public User User { get; set; } = null!;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
