namespace Shared.DTOs.Warehouse;

public class WarehouseRequest
{
    public required string Name { get; set; }
    public string? AgentId { get; set; }

    public string? AddressId { get; set; }
}