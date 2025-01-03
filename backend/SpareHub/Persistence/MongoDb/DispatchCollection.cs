﻿using Domain.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Persistence.MongoDb;

public class DispatchCollection
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public string OriginType { get; set; } = null!;
    public int OriginId { get; set; }
    public string DestinationType { get; set; } = null!;
    public int DestinationId { get; set; }
    public string DispatchStatus { get; set; } = null!;
    public string TransportModeType { get; set; } = null!;
    public string? TrackingNumber { get; set; }
    public DateTime DispatchDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public List<Invoice> Invoices { get; set; } = new();
    public List<OrderReference> Orders { get; set; } = new();
}
