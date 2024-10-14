using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class Order
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public required string OrderNumber { get; set; }
    public string? SupplierOrderNumber { get; set; }
    public int SupplierId { get; set; }
    public int VesselId { get; set; }
    public int WarehouseId { get; set; }
    public DateTime ExpectedReadiness { get; set; }
    public DateTime? ActualReadiness { get; set; }
    public DateTime? ExpectedArrival { get; set; }
    public DateTime? ActualArrival { get; set; }
    public string OrderStatus { get; set; } = null!;

    public Supplier Supplier { get; init; } = null!;
    public Vessel Vessel { get; init; } = null!;
    public Warehouse Warehouse { get; init; } = null!;

    public ICollection<Dispatch> Dispatches { get; init; } = new List<Dispatch>();
}
