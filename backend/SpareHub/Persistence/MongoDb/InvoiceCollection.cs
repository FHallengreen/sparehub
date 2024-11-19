using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Persistence.MongoDb;

public class InvoiceCollection
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public string InvoiceNumber { get; set; } = null!;
    public double Amount { get; set; }
    public Currency Currency { get; set; }
}