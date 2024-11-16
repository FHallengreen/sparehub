namespace Domain.MongoDb;

using MongoDB.Bson.Serialization.Attributes;

public class BoxCollection
{
    [BsonElement("Id")] // Maps the field name in MongoDB
    [BsonRepresentation(MongoDB.Bson.BsonType.String)] 
    public required string Id { get; set; }
    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public double Weight { get; set; }
}
