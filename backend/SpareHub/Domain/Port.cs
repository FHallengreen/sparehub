namespace Domain;

public class Port
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }

    public ICollection<Vessel> Vessels { get; set; } = new List<Vessel>();
}

