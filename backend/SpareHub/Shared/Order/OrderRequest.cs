namespace Shared.Order;

public class OrderRequest
{
    public string OrderNumber { get; set; } = null!;
    public string? SupplierOrderNumber { get; set; }
    public string SupplierId { get; set; } = null!;
    public string VesselId { get; set; } = null!;
    public string WarehouseId { get; set; } = null!;
    public DateTime ExpectedReadiness { get; set; }
    public DateTime? ActualReadiness { get; set; }
    public DateTime? ExpectedArrival { get; set; }
    public DateTime? ActualArrival { get; set; }
    public string OrderStatus { get; set; } = null!;
    public List<BoxRequest>? Boxes { get; set; }
}
