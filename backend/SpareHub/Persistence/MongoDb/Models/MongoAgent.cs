using MongoDB.Bson.Serialization.Attributes;

namespace Persistence.MongoDb.Models;

public class MongoAgent
{
    [BsonElement("Id")]
    public string AgentId { get; set; } = null!;
    public string Name { get; set; } = null!;
}