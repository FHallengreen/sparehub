using Domain.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Persistence.MongoDb.Models;
using OrderStatus = Persistence.MongoDb.Enums.OrderStatus;

namespace Persistence.MongoDb;

public class OrderCollection
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public string OrderNumber { get; set; } = null!;
    public string? SupplierOrderNumber { get; set; }
    public MongoSupplier Supplier { get; set; } = null!;
    public MongoVessel Vessel { get; set; } = null!;
    public MongoOwner VesselOwner { get; set; } = null!;
    public MongoWarehouse Warehouse { get; set; } = null!;
    public MongoAgent Agent { get; set; } = null!;
    public DateTime ExpectedReadiness { get; set; }
    public DateTime? ActualReadiness { get; set; }
    public DateTime? ExpectedArrival { get; set; }
    public DateTime? ActualArrival { get; set; }
    public OrderStatus OrderStatus { get; set; }
}