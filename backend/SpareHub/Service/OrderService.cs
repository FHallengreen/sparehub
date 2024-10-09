using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Shared;

namespace Service;

public class OrderService (SpareHubDbContext dbContext) : IOrderService
{
    public async Task<IEnumerable<Order>> GetOrders()
    {
        return await dbContext.Orders.ToListAsync();
    }

    public async Task CreateOrder(OrderRequest orderRequest)
    {
        var order = new Order
        {
            OrderNumber = orderRequest.OrderNumber,
            SupplierOrderNumber = orderRequest.SupplierOrderNumber,
            SupplierId = orderRequest.SupplierId,
            VesselId = orderRequest.VesselId,
            WarehouseId = orderRequest.WarehouseId,
            ExpectedReadiness = orderRequest.ExpectedReadiness,
            ActualReadiness = orderRequest.ActualReadiness,
            ExpectedArrival = orderRequest.ExpectedArrival,
            ActualArrival = orderRequest.ActualArrival,
            OrderStatus = orderRequest.OrderStatus
        };
        await dbContext.Orders.AddAsync(order);
        await dbContext.SaveChangesAsync();
    }
}
