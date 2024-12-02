using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Owner;

public class OwnerRequest
{
    
    [Required(ErrorMessage = "Name is required.")]
    public required string Name { get; set; }
}