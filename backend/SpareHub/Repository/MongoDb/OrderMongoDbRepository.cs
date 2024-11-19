using AutoMapper;
using Domain.Models;
using MongoDB.Driver;
using Persistence.MongoDb;
using Repository.Interfaces;

namespace Repository.MongoDb;

public class OrderMongoDbRepository (IMongoCollection<OrderCollection> collection, IMapper mapper) : IOrderRepository
{
    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
            var orderCollection = await collection.Find(order => true).ToListAsync();
            return mapper.Map<IEnumerable<Order>>(orderCollection);
    }

    public async Task<Order?> GetOrderByIdAsync(string orderId)
    {
        var order = await collection.Find(order => orderId == order.Id).FirstOrDefaultAsync();
        return mapper.Map<Order>(order);
    }

    public async Task<IEnumerable<Order>> GetNonCancelledOrdersAsync()
    {
        var orderCollection = await collection.Find(order => true).ToListAsync();
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
        return await collection.Distinct<string>("OrderStatus", FilterDefinition<OrderCollection>.Empty).ToListAsync();
    }
}