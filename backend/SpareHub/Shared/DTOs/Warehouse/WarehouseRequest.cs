namespace Shared;

public class WarehouseRequest
{
    public required string Name { get; set; }
    public string? AgentId { get; set; }

    public required string AddressId { get; set; }
}