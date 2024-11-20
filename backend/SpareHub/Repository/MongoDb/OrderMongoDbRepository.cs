using AutoMapper;
using Domain.Models;
using Domain.MongoDb;
using MongoDB.Driver;
using Repository.Interfaces;

namespace Repository.MongoDb;

public class OrderMongoDbRepository (IMongoCollection<OrderDocument> collection, IMapper mapper) : IOrderRepository
{
    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        var orderCollection = await collection.Find(order => true).ToListAsync();
        return mapper.Map<IEnumerable<Order>>(orderCollection);
    }

    public Task<Order?> GetOrderByIdAsync(string orderId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Order>> GetNonCancelledOrdersAsync()
    {
        var filter = Builders<OrderDocument>.Filter.Ne(order => order.OrderStatus, "Cancelled");

        var orderCollection = await collection.Find(filter).ToListAsync();

        return mapper.Map<IEnumerable<Order>>(orderCollection);

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
        return await collection.Distinct<string>("OrderStatus", FilterDefinition<OrderDocument>.Empty).ToListAsync();
    }
}