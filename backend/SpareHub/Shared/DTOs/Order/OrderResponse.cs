using Shared.DTOs.Supplier;
using Shared.DTOs.Vessel;
using Shared.DTOs.Warehouse;

namespace Shared.DTOs.Order;

public record OrderResponse
{
    public string Id { get; set; } = null!;
    public string OrderNumber { get; set; } = null!;
    public string? SupplierOrderNumber { get; set; }
    public DateTime ExpectedReadiness { get; set; }
    public DateTime? ActualReadiness { get; set; }
    public DateTime? ExpectedArrival { get; set; }
    public DateTime? ActualArrival { get; set; }
    public string OrderStatus { get; set; } = null!;
    public string? TrackingNumber { get; set; } // New property
    public string? Transporter { get; set; } // New property
    public SupplierResponse Supplier { get; set; } = null!;
    public VesselResponse Vessel { get; set; } = null!;
    public WarehouseResponse Warehouse { get; set; } = null!;
    public List<BoxResponse> Boxes { get; set; } = new();
}
