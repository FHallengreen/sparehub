namespace Shared.DTOs.Vessel;

public class VesselRequest
{
    public string? Id { get; set; }
    public required string Name { get; set; }
    public string? ImoNumber { get; set; }
    public string? Flag { get; set; }
    public required string OwnerId { get; set; }
    
    
}
