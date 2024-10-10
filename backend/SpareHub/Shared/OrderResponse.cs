namespace Shared;

public class OrderResponse
{
    public int Id { get; init; }
    public required string OrderNumber { get; init; }
    public string? SupplierOrderNumber { get; init; } 
    public DateTime ExpectedReadiness { get; init; }
    public DateTime? ActualReadiness { get; init; }
    public DateTime? ExpectedArrival { get; init; }
    public DateTime? ActualArrival { get; init; }
    public required string SupplierName { get; init; }
    public required string VesselName { get; init; }
    public required string WarehouseName { get; init; }
    public string OrderStatus { get; init; } = null!;
    
}


