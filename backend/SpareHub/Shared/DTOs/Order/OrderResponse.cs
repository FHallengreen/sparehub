using Domain;
using Shared.Order;

namespace Shared;

public class OrderResponse
{
    public string Id { get; set; } = null!;
    public string OrderNumber { get; set; } = null!;
    public string? SupplierOrderNumber { get; set; }
    public DateTime ExpectedReadiness { get; set; }
    public DateTime? ActualReadiness { get; set; }
    public DateTime? ExpectedArrival { get; set; }
    public DateTime? ActualArrival { get; set; }
    public string OrderStatus { get; set; } = null!;
    public SupplierResponse Supplier { get; set; } = null!;
    public VesselResponse Vessel { get; set; } = null!;
    public WarehouseResponse Warehouse { get; set; } = null!;
    public string Owner { get; set; } = null!;
    public List<BoxResponse> Boxes { get; set; } = new();
}