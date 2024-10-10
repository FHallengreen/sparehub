using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class Order
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public required string OrderNumber { get; init; }
    public string? SupplierOrderNumber { get; init; }
    public int SupplierId { get; init; }
    public int VesselId { get; init; }
    public int WarehouseId { get; init; }
    public DateTime ExpectedReadiness { get; init; }
    public DateTime? ActualReadiness { get; init; }
    public DateTime? ExpectedArrival { get; init; }
    public DateTime? ActualArrival { get; init; }
    public string OrderStatus { get; init; } = null!;

    public Supplier Supplier { get; init; } = null!;
    public Vessel Vessel { get; init; } = null!;
    public Warehouse Warehouse { get; init; } = null!;

    public ICollection<Dispatch> Dispatches { get; set; } = new List<Dispatch>();
}
