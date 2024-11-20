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
    public string SupplierName { get; set; } = null!;
    public int VesselId { get; set; }
    public string VesselName { get; set; } = null!;
    public int VesselOwnerId { get; set; }
    public string VesselOwnerName { get; set; } = null!;
    public int WarehouseId { get; set; }
    public string WarehouseName { get; set; } = null!;

    public int AgentId { get; set; }
    public string AgentName { get; set; } = null!;

    public DateTime ExpectedReadiness { get; set; }
    public DateTime? ActualReadiness { get; set; }
    public DateTime? ExpectedArrival { get; set; }
    public DateTime? ActualArrival { get; set; }

    [BsonRepresentation(BsonType.String)]
    public OrderStatus OrderStatus { get; set; }

    public List<BoxCollection> Boxes { get; set; } = new();
    public List<DispatchCollection> Dispatches { get; set; } = new();
}
