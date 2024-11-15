using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain;

public class Order
{
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

    public Supplier Supplier { get; set; } = null!;
    public Vessel Vessel { get; set; } = null!;
    public Warehouse Warehouse { get; set; } = null!;

    [JsonIgnore]
    public ICollection<Box> Boxes { get; set; } = new List<Box>();
    [JsonIgnore]
    public ICollection<Dispatch> Dispatches { get; set; } = new List<Dispatch>();
}
