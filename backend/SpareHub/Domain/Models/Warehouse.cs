namespace Domain.Models;

public class Warehouse
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    
    public string AgentId { get; set; } = null!;
    public Agent? Agent { get; set; }
    
    public string AddressId { get; set; } = null!;
    public Address Address { get; set; } = null!;
    
    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Agent: {Agent}, Address: {Address}";
    }
}
