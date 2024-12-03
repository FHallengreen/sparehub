namespace Domain.Models;

public class Operator
{
    public required string Id;
    public required string Name;
    public string? Title;
    public required string UserId { get; set; }
}