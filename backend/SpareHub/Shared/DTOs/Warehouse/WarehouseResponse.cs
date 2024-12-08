using Shared.DTOs.Address;
using Shared.DTOs.Order;

namespace Shared.DTOs.Warehouse;

public class WarehouseResponse
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public AgentResponse? Agent { get; set; }
    public AddressResponse? Address { get; set; }
}
