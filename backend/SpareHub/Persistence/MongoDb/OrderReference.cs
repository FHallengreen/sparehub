using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Persistence.MongoDb;

public class OrderReference
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string OrderId { get; set; } = null!;
}