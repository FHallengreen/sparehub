using MongoDB.Bson.Serialization.Attributes;

namespace Persistence.MongoDb.Models;

public class MongoVessel
{
    [BsonElement("Id")]
    public string VesselId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string ImoNumber { get; set; } = null!;
    public string Flag { get; set; } = null!;
    public MongoOwner Owner { get; set; } = null!;
}