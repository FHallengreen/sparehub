namespace Shared;

public class WarehouseResponse
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public AgentResponse? Agent { get; set; }
}
