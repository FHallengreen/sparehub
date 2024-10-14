using Domain;

namespace Shared;

public class OrderRequest
{
    public required string OrderNumber { get; init; }
    public string? SupplierOrderNumber { get; init; }
    public DateTime ExpectedReadiness { get; init; }
    public DateTime? ActualReadiness { get; init; }
    public DateTime? ExpectedArrival { get; init; }
    public DateTime? ActualArrival { get; init; }
    public required int SupplierId { get; init; }
    public required int VesselId { get; init; }
    public required int WarehouseId { get; init; }
    public string OrderStatus { get; init; } = null!;
    public List<Box>? Boxes { get; set; }
}
