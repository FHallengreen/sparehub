using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.VesselAtPort;

public class VesselAtPortRequest
{
    [Required (ErrorMessage = "PortId is required.")]
    public required string PortId { get; set; }
    
    [Required (ErrorMessage = "Vessel Id is required.")]
    public required string VesselId { get; set; }
    
    [Required (ErrorMessage = "Arrival Date is required.")]
    public required string ArrivalDate { get; set; }
    
    [Required (ErrorMessage = "Departure Date is required.")]
    public required string DepartureDate { get; set; }
}
