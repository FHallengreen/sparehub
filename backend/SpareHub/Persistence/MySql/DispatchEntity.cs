using System.ComponentModel.DataAnnotations.Schema;
using Domain.MySql;

namespace Persistence.MySql;

public class DispatchEntity
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
    public UserEntity userEntity { get; set; } = null!;
    public ICollection<InvoiceEntity> Invoices { get; set; } = new List<InvoiceEntity>();
    public ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
}
