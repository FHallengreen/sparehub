using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.MySql;
public class WarehouseEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public required string Name { get; init; }
    public int AgentId { get; init; }
    [ForeignKey("AgentId")]
    public required AgentEntity  Agent { get; init; } 

    public int AddressId { get; init; }
    [ForeignKey("AddressId")]
    public required AddressEntity Address { get; init; }


    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Agent: {Agent}, Address: {Address}";
    }
}
