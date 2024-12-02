
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Port;

public class PortRequest
{
    [Required(ErrorMessage = "Name is required.")]
    public required string Name { get; set; }
    
    
}