using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Domain.MySql;

namespace Persistence.MySql;
public class BoxEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public double Weight { get; set; }

    public int OrderId { get; set; } 

    [JsonIgnore]
    public OrderEntity Order { get; set; } = null!; 
}

