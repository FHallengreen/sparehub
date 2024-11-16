using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Models;

public class Owner
{
    public required string Id { get; set; }
    public required string Name { get; init; }
}
