using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.MySql;
// Agent
public class AgentEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    public required string Name { get; init; }
    
    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}";
    }
}
