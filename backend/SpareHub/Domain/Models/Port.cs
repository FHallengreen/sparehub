
namespace Domain.Models;

public class Port
{
    //ID doesn't need required typing, because the ID will be generated along with the object
    public string Id { get; set; } = null!;
    public required string Name { get; set; } = null!;

}
