namespace Shared;

public class VesselResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? ImoNumber { get; set; }
    public string? Flag { get; set; }
    public OwnerResponse Owner { get; set; }
    public DateTime? ArrivalDate { get; set; }
    public DateTime? DepartureDate { get; set; }
}
