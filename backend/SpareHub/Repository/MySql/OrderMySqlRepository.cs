using AutoMapper;
using Domain.Models;
using Domain.MySql;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Repository.Interfaces;

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

    public async Task<Order?> GetOrderByIdAsync(string orderId)
    {
        if (!int.TryParse(orderId, out var id))
            return null;

        var orderEntity = await dbContext.Orders
            .Include(o => o.Supplier)
            .Include(o => o.Vessel)
            .ThenInclude(v => v.Owner)
            .Include(o => o.Warehouse)
            .ThenInclude(w => w.Agent)
            .Include(o => o.Boxes)
            .FirstOrDefaultAsync(o => o.Id == id);

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
        int orderId = int.Parse(order.Id);
        var existingOrderEntity = await dbContext.Orders
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (existingOrderEntity == null)
        {
            throw new KeyNotFoundException("Order not found");
        }

        mapper.Map(order, existingOrderEntity);

        await dbContext.SaveChangesAsync();
    }


    public async Task DeleteOrderAsync(string orderId)
    {
        if (!int.TryParse(orderId, out var id))
            return;

        var orderEntity = await dbContext.Orders.FindAsync(id);
        if (orderEntity != null)
        {
            dbContext.Orders.Remove(orderEntity);
            await dbContext.SaveChangesAsync();
        }
    }

    public Task<List<string>> GetAllOrderStatusesAsync()
    {
        return dbContext.OrderStatus.Select(s => s.Status).ToListAsync();
    }
}
