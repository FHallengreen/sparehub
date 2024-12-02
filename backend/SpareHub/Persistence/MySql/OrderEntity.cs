using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Persistence.MySql;

public class OrderEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string OrderNumber { get; set; } = null!;
    public string? SupplierOrderNumber { get; set; }
    public int SupplierId { get; set; }
    public int VesselId { get; set; }
    public int WarehouseId { get; set; }
    public DateTime ExpectedReadiness { get; set; }
    public DateTime? ActualReadiness { get; set; }
    public DateTime? ExpectedArrival { get; set; }
    public DateTime? ActualArrival { get; set; }
    public string OrderStatus { get; set; } = null!;

    public SupplierEntity Supplier { get; set; } = null!;
    public VesselEntity Vessel { get; set; } = null!;
    public WarehouseEntity Warehouse { get; set; } = null!;

    [JsonIgnore]
    public ICollection<BoxEntity> Boxes { get; set; } = new List<BoxEntity>();
    [JsonIgnore]
    public ICollection<DispatchEntity> Dispatches { get; set; } = new List<DispatchEntity>();
}
