
namespace Domain.Models;

public class Owner
{
    //ID doesn't need required typing, because the ID will be generated along with the object
    public string Id { get; init; }
    public required string Name { get; set; }
}
