namespace Domain.Models;

public class Order
{
    public string Id { get; set; } = null!;
    public string OrderNumber { get; set; } = null!;
    public string? SupplierOrderNumber { get; set; }
    public string SupplierId { get; set; } = null!;
    public Supplier Supplier { get; set; } = null!;
    public string VesselId { get; set; } = null!;
    public Vessel Vessel { get; set; } = null!;
    public string WarehouseId { get; set; } = null!;
    public Warehouse Warehouse { get; set; } = null!;
    public DateTime ExpectedReadiness { get; set; }
    public DateTime? ActualReadiness { get; set; }
    public DateTime? ExpectedArrival { get; set; }
    public DateTime? ActualArrival { get; set; }
    public string? TrackingNumber { get; set; }
    public string? Transporter { get; set; }
    public string OrderStatus { get; set; } = null!;
    public List<Box> Boxes { get; set; } = new();
}
