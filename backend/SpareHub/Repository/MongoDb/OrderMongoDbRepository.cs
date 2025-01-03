using AutoMapper;
using Domain.Models;
using MongoDB.Driver;
using Persistence.MongoDb;
using Repository.Interfaces;
using OrderStatus = Persistence.MongoDb.Enums.OrderStatus;

namespace Repository.MongoDb;

public class OrderMongoDbRepository(
    IMongoCollection<OrderCollection> collection,
    IBoxRepository boxRepository,
    IMapper mapper) : IOrderRepository
{
    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        var orderCollections = await collection.Find(order => true).ToListAsync();
        var orders = mapper.Map<List<Order>>(orderCollections);

        foreach (var order in orders)
        {
            var boxes = await boxRepository.GetBoxesByOrderIdAsync(order.Id);
            order.Boxes = boxes;
        }

        return orders;
    }

    public async Task<Order?> GetOrderByIdAsync(string orderId)
    {
        var orderCollection = await collection.Find(order => order.Id == orderId).FirstOrDefaultAsync();
        if (orderCollection == null) return null;

        var order = mapper.Map<Order>(orderCollection);
        var boxes = await boxRepository.GetBoxesByOrderIdAsync(orderId);
        order.Boxes = boxes;

        return order;
    }

    public async Task<IEnumerable<Order>> GetActiveOrders()
    {
        var filter = Builders<OrderCollection>.Filter.Nin(order => order.OrderStatus,
            [OrderStatus.Cancelled, OrderStatus.Delivered]);

        var orderCollections = await collection.Find(filter).ToListAsync();
        var orders = mapper.Map<List<Order>>(orderCollections);

        foreach (var order in orders)
        {
            var boxes = await boxRepository.GetBoxesByOrderIdAsync(order.Id);
            order.Boxes = boxes;
        }

        return orders;
    }

    public async Task CreateOrderAsync(Order order)
    {
        var orderCollection = mapper.Map<OrderCollection>(order);

        await collection.InsertOneAsync(orderCollection);
    }

    public async Task UpdateOrderAsync(Order order)
    {
        var orderCollection = mapper.Map<OrderCollection>(order);

        var filter = Builders<OrderCollection>.Filter.Eq(o => o.Id, order.Id);
        await collection.ReplaceOneAsync(filter, orderCollection);
    }

    public async Task DeleteOrderAsync(string orderId)
    {
        var filter = Builders<OrderCollection>.Filter.Eq(o => o.Id, orderId);
        await collection.DeleteOneAsync(filter);
    }

    public async Task<List<string>> GetAllOrderStatusesAsync()
    {
        return await collection.Distinct<string>("OrderStatus", FilterDefinition<OrderCollection>.Empty).ToListAsync();
    }

    public async Task<List<Order>> GetOrdersByIdsAsync(List<string> orderIds)
    {
        var filter = Builders<OrderCollection>.Filter.In(order => order.Id, orderIds);
        var orderCollections = await collection.Find(filter).ToListAsync();
        var orders = mapper.Map<List<Order>>(orderCollections);

        foreach (var order in orders)
        {
            var boxes = await boxRepository.GetBoxesByOrderIdAsync(order.Id);
            order.Boxes = boxes;
        }

        return orders;
    }
}