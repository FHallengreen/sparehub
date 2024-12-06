using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.VesselAtPort;

public class VesselAtPortRequest
{
    [Required (ErrorMessage = "PortId is required.")]
    public required string PortId { get; set; }
    
    [Required (ErrorMessage = "Vessel Id is required.")]
    public required string VesselId { get; set; }
    
    public DateTime ArrivalDate { get; set; }
    
    public DateTime DepartureDate { get; set; }
}
