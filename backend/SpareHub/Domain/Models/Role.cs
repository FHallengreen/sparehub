using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class Role
{
    public required string Id { get; set; }
    public required string Title { get; set; }
}
