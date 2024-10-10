using System.ComponentModel.DataAnnotations;
using Domain;

namespace Shared;

public class OrderRequest
{
    public required string OrderNumber { get; set; }

    public string? SupplierOrderNumber { get; set; }

    public required int SupplierId { get; set; }

    public required int VesselId { get; set; }

    public required int WarehouseId { get; set; }

    public required DateTime ExpectedReadiness { get; set; }

    public DateTime? ActualReadiness { get; set; }

    public DateTime? ExpectedArrival { get; set; }

    public DateTime? ActualArrival { get; set; }

    public required string OrderStatus { get; set; }
}
