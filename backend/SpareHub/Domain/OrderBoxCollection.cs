using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain;

public class OrderBoxCollection
{
        [BsonId]
        public ObjectId Id { get; set; } 
        public int OrderId { get; init; }
        public List<Box> Boxes { get; set; } = [];
}
