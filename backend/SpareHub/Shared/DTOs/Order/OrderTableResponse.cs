using Domain.Models;

namespace Shared.DTOs.Order;

public record OrderTableResponse
{
    public string Id { get; set; } = null!;
    public string OrderNumber { get; set; } = null!;
    public string SupplierName { get; set; } = null!;
    public string OwnerName { get; set; } = null!;
    public string VesselName { get; set; } = null!;
    public string WarehouseName { get; set; } = null!;
    public OrderStatus OrderStatus { get; set; }
    public int Boxes { get; set; }
    public double TotalWeight { get; set; }
    public double TotalVolume { get; set; }
    public double TotalVolumetricWeight { get; set; }
}
