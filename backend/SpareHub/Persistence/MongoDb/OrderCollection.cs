using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Persistence.MongoDb;

public class OrderCollection
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public string OrderNumber { get; set; } = null!;
    public string? SupplierOrderNumber { get; set; }
    public int SupplierId { get; set; }
    public int VesselId { get; set; }
    public int VesselOwnerId { get; set; }
    public int WarehouseId { get; set; }
    public int AgentId { get; set; }
    public DateTime ExpectedReadiness { get; set; }
    public DateTime? ActualReadiness { get; set; }
    public DateTime? ExpectedArrival { get; set; }
    public DateTime? ActualArrival { get; set; }
    public OrderStatus OrderStatus { get; set; }
}