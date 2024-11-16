using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain;

using System.ComponentModel.DataAnnotations.Schema;

public class Box
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public double Weight { get; set; }

    [JsonIgnore]  public ICollection<Order> Orders { get; set; } = new List<Order>();
}
