using MongoDB.Bson.Serialization.Attributes;

namespace Persistence.MongoDb.Models;

public class MongoAddress
{
    [BsonElement("Id")]
    public string AddressId { get; set; } = null!;
    public required string AddressLine { get; set; }
    public required string PostalCode { get; set; }
    public required string Country { get; set; }
}