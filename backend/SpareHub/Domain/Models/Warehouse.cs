using System.ComponentModel.DataAnnotations.Schema;
using Domain.MySql;

namespace Domain.Models;

public class Warehouse
{
    public required string Id { get; set; }
    public required string Name { get; init; }
    public Agent Agent { get; init; } = null!;
}
