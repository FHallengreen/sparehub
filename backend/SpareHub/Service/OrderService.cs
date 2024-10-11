using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using Persistence;
using Shared;

namespace Service;

public class OrderService(SpareHubDbContext dbContext, IMemoryCache memory, IMongoCollection<OrderBoxCollection> collection) : IOrderService
{
    private const string OrderStatusCacheKey = "OrderStatuses";

   public async Task<IEnumerable<OrderResponse>> GetOrders(List<string>? searchTerms = null)
{
    var query = dbContext.Orders
        .Include(o => o.Supplier)
        .Include(o => o.Vessel)
        .ThenInclude(v => v.Owner)
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
            OwnerName = o.Vessel.Owner.Name,
            VesselName = o.Vessel.Name,
            WarehouseName = o.Warehouse.Name,
            OrderStatus = o.OrderStatus
        });

    var orderStatusFilter = searchTerms?.FirstOrDefault(term => term.Equals("Cancelled", StringComparison.OrdinalIgnoreCase) ||
                                    term.Equals("Ready", StringComparison.OrdinalIgnoreCase) ||
                                    term.Equals("Inbound", StringComparison.OrdinalIgnoreCase) ||
                                    term.Equals("Stock", StringComparison.OrdinalIgnoreCase));

    query = orderStatusFilter != null
        ? query.Where(o => o.OrderStatus.Equals(orderStatusFilter, StringComparison.OrdinalIgnoreCase))
        : query.Where(o => !o.OrderStatus.Equals("Cancelled", StringComparison.OrdinalIgnoreCase));

    var orders = await query.ToListAsync();

    foreach (var order in orders)
    {
        var orderBox = await collection.Find(o => o.OrderId == order.Id).FirstOrDefaultAsync();

        if (orderBox != null)
        {
            order.Boxes = orderBox.Boxes.Count;
            order.TotalWeight = orderBox.Boxes.Sum(p => p.Weight);
        }
        else
        {
            order.Boxes = null;
            order.TotalWeight = null;
        }
    }

    if (searchTerms is not { Count: > 0 }) return orders;
    var nonStatusTerms = searchTerms.Where(t => t != orderStatusFilter).ToList();
    return nonStatusTerms.Aggregate(orders, (current, term) => current.Where(o =>
        o.WarehouseName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
        o.VesselName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
        o.OrderNumber.Contains(term, StringComparison.OrdinalIgnoreCase) ||
        o.SupplierName.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList());
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
        if (memory.TryGetValue(OrderStatusCacheKey, out List<string>? cachedStatuses)) return cachedStatuses;
        // Fetch just the status strings from the database
        cachedStatuses = await dbContext.OrderStatus.Select(s => s.Status).ToListAsync();

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromHours(24));

        memory.Set(OrderStatusCacheKey, cachedStatuses, cacheEntryOptions);

        return cachedStatuses;
    }
}
