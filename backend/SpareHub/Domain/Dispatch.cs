using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class Dispatch
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

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

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}