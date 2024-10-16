using Domain;

namespace Shared;

public class OrderResponse
{
    public int Id { get; set; }
    public required string OrderNumber { get; set; }
    public string? SupplierOrderNumber { get; set; }
    public DateTime ExpectedReadiness { get; set; }
    public DateTime? ActualReadiness { get; set; }
    public DateTime? ExpectedArrival { get; set; }
    public DateTime? ActualArrival { get; set; }
    public required SupplierResponse Supplier { get; set; }
    public required OwnerResponse Owner { get; set; }
    public required VesselResponse Vessel { get; set; }
    public required WarehouseResponse Warehouse { get; set; }
    public required AgentResponse Agent { get; set; }
    public required string OrderStatus { get; set; }

    public List<Box>? Boxes { get; set; }
    
}
