using MongoDB.Bson.Serialization.Attributes;

namespace Persistence.MongoDb.Models;

public class MongoWarehouse
{
    [BsonElement("Id")]
    public string WarehouseId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public MongoAgent? Agent { get; set; }
    public MongoAddress Address { get; set; } = null!;
}