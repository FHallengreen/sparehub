using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain;

public class OrderBoxCollection
{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("OrderId")]
        public int OrderId { get; set; }

        [BsonElement("Boxes")]
        public List<Box> Boxes { get; set; }
}
