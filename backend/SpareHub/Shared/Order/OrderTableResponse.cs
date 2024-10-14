namespace Shared;

public class OrderTableResponse
{
    public int Id { get; init; }
    public required string OrderNumber { get; init; }
    public required string SupplierName { get; init; }
    public required string OwnerName { get; init; }
    public required string VesselName { get; init; }
    public required string WarehouseName { get; init; }
    public string OrderStatus { get; init; } = null!;

    public int? Boxes { get; set; }
    public double? TotalWeight { get; set; }
}
