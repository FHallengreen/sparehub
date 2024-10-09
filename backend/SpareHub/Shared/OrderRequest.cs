namespace Shared;

public class OrderRequest(
    string orderNumber,
    string? supplierOrderNumber,
    int supplierId,
    int vesselId,
    int warehouseId,
    DateTime expectedReadiness,
    DateTime? actualReadiness,
    DateTime? expectedArrival,
    DateTime? actualArrival,
    string orderStatus)
{
    
    public string OrderNumber { get; set; } = orderNumber;
    public string? SupplierOrderNumber { get; set; } = supplierOrderNumber;
    public int SupplierId { get; set; } = supplierId;
    public int VesselId { get; set; } = vesselId;
    public int WarehouseId { get; set; } = warehouseId;
    public DateTime ExpectedReadiness { get; set; } = expectedReadiness;
    public DateTime? ActualReadiness { get; set; } = actualReadiness;
    public DateTime? ExpectedArrival { get; set; } = expectedArrival;
    public DateTime? ActualArrival { get; set; } = actualArrival;
    public string OrderStatus { get; set; } = orderStatus;
}
