using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Order;

public record OrderRequest
{
     [Required(ErrorMessage = "OrderNumber is required.")]
    [MaxLength(45, ErrorMessage = "OrderNumber cannot exceed 45 characters.")]
    public string OrderNumber { get; init; } = null!;

    [MaxLength(45, ErrorMessage = "SupplierOrderNumber cannot exceed 45 characters.")]
    public string? SupplierOrderNumber { get; init; }

    [Required(ErrorMessage = "SupplierId is required.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "SupplierId must be a valid numeric ID.")]
    public string SupplierId { get; init; } = null!;

    [Required(ErrorMessage = "VesselId is required.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "VesselId must be a valid numeric ID.")]
    public string VesselId { get; init; } = null!;

    [Required(ErrorMessage = "WarehouseId is required.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "WarehouseId must be a valid numeric ID.")]
    public string WarehouseId { get; init; } = null!;

    [Required(ErrorMessage = "ExpectedReadiness is required.")]
    [DataType(DataType.Date, ErrorMessage = "ExpectedReadiness must be a valid date.")]
    public DateTime ExpectedReadiness { get; init; }

    [DataType(DataType.Date, ErrorMessage = "ActualReadiness must be a valid date.")]
    public DateTime? ActualReadiness { get; init; }

    [DataType(DataType.Date, ErrorMessage = "ExpectedArrival must be a valid date.")]
    public DateTime? ExpectedArrival { get; init; }

    [DataType(DataType.Date, ErrorMessage = "ActualArrival must be a valid date.")]
    public DateTime? ActualArrival { get; init; }

    [Required(ErrorMessage = "OrderStatus is required.")]
    [MaxLength(20, ErrorMessage = "OrderStatus cannot exceed 20 characters.")]
    public string OrderStatus { get; init; } = null!;

    public List<BoxRequest>? Boxes { get; init; }
}
