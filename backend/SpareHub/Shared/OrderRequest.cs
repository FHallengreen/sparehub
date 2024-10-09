using System.ComponentModel.DataAnnotations;

namespace Shared;

public class OrderRequest
{
    [Required]
    public string OrderNumber { get; set; }

    public string? SupplierOrderNumber { get; set; }

    [Required]
    public int SupplierId { get; set; }

    [Required]
    public int VesselId { get; set; }

    [Required]
    public int WarehouseId { get; set; }

    [Required]
    public DateTime ExpectedReadiness { get; set; }

    public DateTime? ActualReadiness { get; set; }

    public DateTime? ExpectedArrival { get; set; }

    public DateTime? ActualArrival { get; set; }

    [Required]
    public string OrderStatus { get; set; }
}
