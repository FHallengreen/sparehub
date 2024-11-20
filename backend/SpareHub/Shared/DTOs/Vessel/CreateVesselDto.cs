namespace Shared.DTOs.Vessel;

public class CreateVesselDto
{
    public required string Name { get; set; }
    public string? ImoNumber { get; set; }
    public string? Flag { get; set; }
}
