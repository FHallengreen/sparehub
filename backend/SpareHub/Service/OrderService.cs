using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Persistence;
using Shared;

namespace Service;

public class OrderService (SpareHubDbContext dbContext, IMemoryCache memory) : IOrderService
{

    private const string OrderStatusCacheKey = "OrderStatuses";
    
    public async Task<IEnumerable<OrderResponse>> GetOrders(string? search = null)  // Accept search term as parameter
    {
        var query = dbContext.Orders
            .Include(o => o.Supplier)
            .Include(o => o.Vessel)
            .Include(o => o.Warehouse)
            .Select(o => new OrderResponse
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                SupplierOrderNumber = o.SupplierOrderNumber,
                ExpectedReadiness = o.ExpectedReadiness,
                ActualReadiness = o.ActualReadiness,
                ExpectedArrival = o.ExpectedArrival,
                ActualArrival = o.ActualArrival,
                SupplierName = o.Supplier.Name,   
                VesselName = o.Vessel.Name,     
                WarehouseName = o.Warehouse.Name,
                OrderStatus = o.OrderStatus
            });

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(o => 
                o.OrderNumber.Contains(search) ||
                o.SupplierName.Contains(search) ||
                o.VesselName.Contains(search) ||
                o.WarehouseName.Contains(search) ||
                o.OrderStatus.Contains(search)
            );
        }

        return await query.ToListAsync();
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
    
    public async Task<List<string>?> GetAllOrderStatusesAsync()
    {
        if (!memory.TryGetValue(OrderStatusCacheKey, out List<string>? cachedStatuses))
        {
            // Fetch just the status strings from the database
            cachedStatuses = await dbContext.OrderStatus.Select(s => s.Status).ToListAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(24));

            memory.Set(OrderStatusCacheKey, cachedStatuses, cacheEntryOptions);
        }

        return cachedStatuses;
    }


}
