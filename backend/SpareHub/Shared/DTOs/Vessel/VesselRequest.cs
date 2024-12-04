using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Vessel;

public class VesselRequest
{
    [Required(ErrorMessage = "Name is required.")]
    public required string Name { get; set; }
    
    [Required(ErrorMessage = "IMO Number is required.")]
    [RegularExpression("^[0-9]{1,7}$", ErrorMessage = "IMO Number must be between 1 and 7 digits.")]
    public string? ImoNumber { get; set; }
    
    [Required(ErrorMessage = "Flag is required.")]
    [RegularExpression("^[A-Za-z]{2}$", 
        ErrorMessage = "Flag must consist of exactly 2 letters.")]
    public string? Flag { get; set; }
    
    [Required(ErrorMessage = "Owner Id is required.")]
    public required string OwnerId { get; set; }
    
    
}
