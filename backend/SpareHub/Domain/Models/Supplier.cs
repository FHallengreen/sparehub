using System.ComponentModel.DataAnnotations.Schema;
using Domain.MySql;

namespace Domain.Models;

public class Supplier
{
    public required string Id { get; init; }
    public required string Name { get; init; }
}
