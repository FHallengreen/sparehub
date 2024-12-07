using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.MySql;
using Persistence.MySql.SparehubDbContext;
using Repository.Interfaces;
using Shared.Exceptions;

namespace Repository.MySql;

public class OrderMySqlRepository(SpareHubDbContext dbContext, IMapper mapper) : IOrderRepository
{
    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        var orderEntities = await dbContext.Orders
            .Include(o => o.Supplier)
            .Include(o => o.Vessel)
            .ThenInclude(v => v.Owner)
            .Include(o => o.Warehouse)
            .Include(o => o.Boxes)
            .ToListAsync();

        var orders = mapper.Map<IEnumerable<Order>>(orderEntities);
        return orders;
    }

    public async Task<IEnumerable<Order>> GetNotActiveOrders()
    {
        var notActiveOrderEntities = await dbContext.Orders
            .FromSqlRaw("SELECT * FROM not_active_orders")
            .Include(o => o.Supplier)
            .Include(o => o.Vessel)
            .ThenInclude(v => v.Owner)
            .Include(o => o.Warehouse)
            .Include(o => o.Boxes)
            .ToListAsync();

        var orders = mapper.Map<IEnumerable<Order>>(notActiveOrderEntities);
        return orders;
    }

    public async Task<Order?> GetOrderByIdAsync(string orderId)
    {
        var orderEntity = await dbContext.Orders
            .Include(o => o.Supplier)
            .Include(o => o.Vessel)
            .ThenInclude(v => v.Owner)
            .Include(o => o.Warehouse)
            .ThenInclude(w => w.Agent)
            .Include(o => o.Boxes)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id.ToString() == orderId);

        return orderEntity != null ? mapper.Map<Order>(orderEntity) : null;
    }


    public async Task CreateOrderAsync(Order order)
    {
        var orderEntity = mapper.Map<OrderEntity>(order);
        dbContext.Orders.Add(orderEntity);
        await dbContext.SaveChangesAsync();
        order.Id = orderEntity.Id.ToString();
    }

    public async Task UpdateOrderAsync(Order order)
    {
        var orderEntity = mapper.Map<OrderEntity>(order);
        dbContext.Orders.Update(orderEntity);

        await dbContext.SaveChangesAsync();
    }


    public async Task DeleteOrderAsync(string orderId)
    {
        if (!int.TryParse(orderId, out var id))
        {
            throw new ValidationException($"Invalid order ID: {orderId}. Must be a valid integer.");
        }

        var orderEntity = await dbContext.Orders.FindAsync(id);
        if (orderEntity == null)
        {
            throw new NotFoundException($"Order with ID {id} not found.");
        }

        dbContext.Orders.Remove(orderEntity);
        await dbContext.SaveChangesAsync();
    }



    public Task<List<string>> GetAllOrderStatusesAsync()
    {
        return dbContext.OrderStatus.Select(s => s.Status).ToListAsync();
    }
}