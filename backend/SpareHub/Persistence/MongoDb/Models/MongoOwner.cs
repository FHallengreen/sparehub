using MongoDB.Bson.Serialization.Attributes;

namespace Persistence.MongoDb.Models;

public class MongoOwner
{
    [BsonElement("Id")]
    public string OwnerId { get; init; }
    public required string Name { get; set; }
}