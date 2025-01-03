using MongoDB.Bson.Serialization.Attributes;

namespace Persistence.MongoDb.Models;

public class MongoSupplier
{
    [BsonElement("Id")]
    public string SupplierId { get; set; } = null!;

    [BsonElement("Name")]
    public string Name { get; set; } = null!;
}