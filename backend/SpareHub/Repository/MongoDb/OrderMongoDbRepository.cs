﻿using AutoMapper;
using Domain.Models;
using MongoDB.Driver;
using Persistence.MongoDb;
using Repository.Interfaces;
using OrderStatus = Persistence.MongoDb.Enums.OrderStatus;

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

    public async Task<IEnumerable<Order>> GetNotActiveOrders()
    {
        var filter = Builders<OrderCollection>.Filter.Ne(order => order.OrderStatus, OrderStatus.Cancelled);

        var orderCollection = await collection.Find(filter).ToListAsync();
        return mapper.Map<List<Order>>(orderCollection);
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
    
    public async Task<List<Order>> GetOrdersByIdsAsync(List<string> orderIds)
    {
        var filter = Builders<OrderCollection>.Filter.In(order => order.Id, orderIds);
        var orderCollection = await collection.Find(filter).ToListAsync();
        return mapper.Map<List<Order>>(orderCollection);
    }
}