using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Models;

public class Supplier
{
    [BsonElement("Id")]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
}
