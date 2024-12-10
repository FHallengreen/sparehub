namespace Domain.Models;

// Agent
public class Agent
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    
    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}";
    }
}

