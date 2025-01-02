using Domain.Models;
using Neo4j.Driver;
using Repository.Interfaces;

namespace Repository.Neo4J;

public class OrderNeo4JRepository(IDriver driver) : IOrderRepository
{
    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        const string query = @"
    MATCH (o:Order)-[:HAS_STATUS]->(s:Status),
          (o)-[:SUPPLIED_BY]->(sup:Supplier),
          (o)-[:FOR_VESSEL]->(v:Vessel)-[:OWNED_BY]->(own:Owner),
          (o)-[:STORED_AT]->(w:Warehouse)
    OPTIONAL MATCH (o)<-[:BELONGS_TO]-(b:Box)
    RETURN o, s, sup, v, w, own, collect(b) AS boxes";

        return await FetchOrders(query);
    }


    public async Task<Order?> GetOrderByIdAsync(string orderId)
    {
        const string query = @"
    MATCH (o:Order)-[:HAS_STATUS]->(s:Status),
          (o)-[:SUPPLIED_BY]->(sup:Supplier),
          (o)-[:FOR_VESSEL]->(v:Vessel)-[:OWNED_BY]->(own:Owner),
          (o)-[:STORED_AT]->(w:Warehouse)
    OPTIONAL MATCH (o)<-[:BELONGS_TO]-(b:Box)
    WHERE o.id = $orderId
    RETURN o, s, sup, v, w, own, collect(b) AS boxes
    LIMIT 1";

        var session = driver.AsyncSession();
        try
        {
            var result = await session.RunAsync(query, new { orderId });
            var record = await result.SingleAsync();

            if (record == null)
            {
                return null;
            }

            var orderNode = record["o"]?.As<INode>();
            var statusNode = record["s"]?.As<INode>();
            var supplierNode = record["sup"]?.As<INode>();
            var vesselNode = record["v"]?.As<INode>();
            var warehouseNode = record["w"]?.As<INode>();
            var ownerNode = record.ContainsKey("own") ? record["own"]?.As<INode>() : null;
            var boxNodes = record["boxes"]?.As<List<INode>>();

            var boxes = boxNodes?.Select(b => new Box
            {
                Id = b.Properties.ContainsKey("id") ? b["id"]?.As<string>() ?? string.Empty : string.Empty,
                Length = b.Properties.ContainsKey("length") ? b["length"]?.As<int>() ?? 0 : 0,
                Width = b.Properties.ContainsKey("width") ? b["width"]?.As<int>() ?? 0 : 0,
                Height = b.Properties.ContainsKey("height") ? b["height"]?.As<int>() ?? 0 : 0,
                Weight = b.Properties.ContainsKey("weight") ? b["weight"]?.As<double>() ?? 0 : 0
            }).ToList() ?? new List<Box>();

            return new Order
            {
                Id = orderNode?["id"]?.As<string>() ?? string.Empty,
                OrderNumber = orderNode?["orderNumber"]?.As<string>() ?? string.Empty,
                SupplierOrderNumber = orderNode?["supplierOrderNumber"]?.As<string>(),
                SupplierId = supplierNode?["id"]?.As<string>() ?? string.Empty,
                Supplier = new Supplier
                {
                    Id = supplierNode?["id"]?.As<string>() ?? string.Empty,
                    Name = supplierNode?["name"]?.As<string>() ?? "Unknown Supplier"
                },
                VesselId = vesselNode?["id"]?.As<string>() ?? string.Empty,
                Vessel = new Vessel
                {
                    Id = vesselNode?["id"]?.As<string>() ?? string.Empty,
                    Name = vesselNode?["name"]?.As<string>() ?? "Unknown Vessel",
                    ImoNumber = vesselNode?["imoNumber"]?.As<string>() ?? string.Empty,
                    Flag = vesselNode?["flag"]?.As<string>() ?? string.Empty,
                    Owner = ownerNode != null
                        ? new Owner
                        {
                            Id = ownerNode["id"]?.As<string>() ?? string.Empty,
                            Name = ownerNode["name"]?.As<string>() ?? "Unknown Owner"
                        }
                        : new Owner { Id = string.Empty, Name = "Unknown Owner" }
                },
                WarehouseId = warehouseNode?["id"]?.As<string>() ?? string.Empty,
                Warehouse = new Warehouse
                {
                    Id = warehouseNode?["id"]?.As<string>() ?? string.Empty,
                    Name = warehouseNode?["name"]?.As<string>() ?? "Unknown Warehouse"
                },
                OrderStatus = statusNode?["name"]?.As<string>() ?? "Unknown Status",
                ExpectedReadiness = orderNode != null && orderNode.Properties.ContainsKey("expectedReadiness")
                    ? DateTime.Parse(orderNode["expectedReadiness"].As<string>())
                    : default,
                ActualReadiness = orderNode != null && orderNode.Properties.ContainsKey("actualReadiness")
                    ? DateTime.Parse(orderNode["actualReadiness"].As<string>())
                    : null,
                ExpectedArrival = orderNode != null && orderNode.Properties.ContainsKey("expectedArrival")
                    ? DateTime.Parse(orderNode["expectedArrival"].As<string>())
                    : default,
                ActualArrival = orderNode != null && orderNode.Properties.ContainsKey("actualArrival")
                    ? DateTime.Parse(orderNode["actualArrival"].As<string>())
                    : null,
                TrackingNumber = orderNode != null && orderNode.Properties.ContainsKey("trackingNumber")
                    ? orderNode["trackingNumber"].As<string>()
                    : null,
                Transporter = orderNode != null && orderNode.Properties.ContainsKey("transporter")
                    ? orderNode["transporter"].As<string>()
                    : null,
                Boxes = boxes
            };
        }
        finally
        {
            await session.CloseAsync();
        }
    }

    public async Task<IEnumerable<Order>> GetNotActiveOrders()
    {
        const string query = @"
    MATCH (o:Order)-[:HAS_STATUS]->(s:Status), 
          (o)-[:SUPPLIED_BY]->(sup:Supplier),
          (o)-[:FOR_VESSEL]->(v:Vessel)-[:OWNED_BY]->(own:Owner),
          (o)-[:STORED_AT]->(w:Warehouse)
    OPTIONAL MATCH (o)<-[:BELONGS_TO]-(b:Box)
    WHERE NOT s.name IN ['Cancelled', 'Delivered']
    RETURN o, sup, v, w, s, own, collect(b) AS boxes";

        return await FetchOrders(query);
    }


    private async Task<IEnumerable<Order>> FetchOrders(string query)
    {
        var session = driver.AsyncSession();
        try
        {
            var result = await session.RunAsync(query);
            var orders = new List<Order>();

            await result.ForEachAsync(record =>
            {
                var orderNode = record["o"]?.As<INode>();
                var statusNode = record["s"]?.As<INode>();
                var supplierNode = record["sup"]?.As<INode>();
                var vesselNode = record["v"]?.As<INode>();
                var warehouseNode = record["w"]?.As<INode>();
                var ownerNode = record.ContainsKey("own") ? record["own"]?.As<INode>() : null;
                var boxNodes = record["boxes"]?.As<List<INode>>();

                var boxes = boxNodes?.Select(b => new Box
                {
                    Id = b.Properties.ContainsKey("id") ? b["id"]?.As<string>() ?? string.Empty : string.Empty,
                    Length = b.Properties.ContainsKey("length") ? b["length"]?.As<int>() ?? 0 : 0,
                    Width = b.Properties.ContainsKey("width") ? b["width"]?.As<int>() ?? 0 : 0,
                    Height = b.Properties.ContainsKey("height") ? b["height"]?.As<int>() ?? 0 : 0,
                    Weight = b.Properties.ContainsKey("weight") ? b["weight"]?.As<double>() ?? 0 : 0
                }).ToList() ?? [];

                orders.Add(new Order
                {
                    Id = orderNode?["id"]?.As<string>() ?? string.Empty,
                    OrderNumber = orderNode?["orderNumber"]?.As<string>() ?? string.Empty,
                    SupplierOrderNumber = orderNode?["supplierOrderNumber"]?.As<string>(),
                    SupplierId = supplierNode?["id"]?.As<string>() ?? string.Empty,
                    Supplier = new Supplier
                    {
                        Id = supplierNode?["id"]?.As<string>() ?? string.Empty,
                        Name = supplierNode?["name"]?.As<string>() ?? "Unknown Supplier"
                    },
                    VesselId = vesselNode?["id"]?.As<string>() ?? string.Empty,
                    Vessel = new Vessel
                    {
                        Id = vesselNode?["id"]?.As<string>() ?? string.Empty,
                        Name = vesselNode?["name"]?.As<string>() ?? "Unknown Vessel",
                        ImoNumber = vesselNode?["imoNumber"]?.As<string>() ?? string.Empty,
                        Flag = vesselNode?["flag"]?.As<string>() ?? string.Empty,
                        Owner = ownerNode != null
                            ? new Owner
                            {
                                Id = ownerNode.Properties.ContainsKey("id")
                                    ? ownerNode["id"]?.As<string>() ?? "Unknown Owner ID"
                                    : "Missing ID",
                                Name = ownerNode.Properties.ContainsKey("name")
                                    ? ownerNode["name"]?.As<string>() ?? "Unknown Owner Name"
                                    : "Missing Name"
                            }
                            : new Owner { Id = string.Empty, Name = "Unknown Owner" }
                    },
                    WarehouseId = warehouseNode?["id"]?.As<string>() ?? string.Empty,
                    Warehouse = new Warehouse
                    {
                        Id = warehouseNode?["id"]?.As<string>() ?? string.Empty,
                        Name = warehouseNode?["name"]?.As<string>() ?? "Unknown Warehouse"
                    },
                    OrderStatus = statusNode?["name"]?.As<string>() ?? "Unknown Status",
                    ExpectedReadiness = orderNode != null && orderNode.Properties.ContainsKey("expectedReadiness")
                        ? DateTime.Parse(orderNode["expectedReadiness"].As<string>())
                        : default,
                    ActualReadiness = orderNode != null && orderNode.Properties.ContainsKey("actualReadiness")
                        ? DateTime.Parse(orderNode["actualReadiness"].As<string>())
                        : null,
                    ExpectedArrival = orderNode != null && orderNode.Properties.ContainsKey("expectedArrival")
                        ? DateTime.Parse(orderNode["expectedArrival"].As<string>())
                        : default,
                    ActualArrival = orderNode != null && orderNode.Properties.ContainsKey("actualArrival")
                        ? DateTime.Parse(orderNode["actualArrival"].As<string>())
                        : null,
                    TrackingNumber = orderNode != null && orderNode.Properties.ContainsKey("trackingNumber")
                        ? orderNode["trackingNumber"].As<string>()
                        : null,
                    Transporter = orderNode != null && orderNode.Properties.ContainsKey("transporter")
                        ? orderNode["transporter"].As<string>()
                        : null,
                    Boxes = boxes
                });
            });

            return orders;
        }
        finally
        {
            await session.CloseAsync();
        }
    }

    public Task CreateOrderAsync(Order order)
    {
        throw new NotImplementedException();
    }

    public Task UpdateOrderAsync(Order order)
    {
        throw new NotImplementedException();
    }

    public Task DeleteOrderAsync(string orderId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<string>> GetAllOrderStatusesAsync()
    {
        const string query = "MATCH (s:Status) RETURN s.name AS status";

        var session = driver.AsyncSession();
        try
        {
            var result = await session.RunAsync(query);
            var statuses = new List<string>();

            await result.ForEachAsync(record => { statuses.Add(record["status"].As<string>()); });

            return statuses;
        }
        finally
        {
            await session.CloseAsync();
        }
    }


    public Task<List<Order>> GetOrdersByIdsAsync(List<string> orderIds)
    {
        throw new NotImplementedException();
    }
}