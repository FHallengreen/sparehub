using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using Persistence;
using Shared;

namespace Service;

public class OrderService(
    SpareHubDbContext dbContext,
    IMemoryCache memory,
    IMongoCollection<OrderBoxCollection> collection,
    IBoxService boxService) : IOrderService
{
    private const string OrderStatusCacheKey = "OrderStatuses";

    public async Task<IEnumerable<OrderTableResponse>> GetOrders(List<string>? searchTerms = null)
    {
        var availableStatuses = await GetAllOrderStatusesAsync();

        var query = dbContext.Orders
            .Include(o => o.Supplier)
            .Include(o => o.Vessel)
            .ThenInclude(v => v.Owner)
            .Include(o => o.Warehouse)
            .Select(o => new OrderTableResponse
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                SupplierName = o.Supplier.Name,
                OwnerName = o.Vessel.Owner.Name,
                VesselName = o.Vessel.Name,
                WarehouseName = o.Warehouse.Name,
                OrderStatus = o.OrderStatus
            });

        var orderStatusFilter = searchTerms?.FirstOrDefault(term =>
            availableStatuses != null && availableStatuses.Contains(term, StringComparer.OrdinalIgnoreCase));

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

        var nonStatusTerms = searchTerms.Where(t =>
            availableStatuses != null && !availableStatuses.Contains(t, StringComparer.OrdinalIgnoreCase)).ToList();
        return nonStatusTerms.Aggregate(orders, (current, term) => current.Where(o =>
            o.WarehouseName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
            o.VesselName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
            o.OrderNumber.Contains(term, StringComparison.OrdinalIgnoreCase) ||
            o.SupplierName.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList());
    }

    public async Task CreateOrder(OrderRequest orderTableRequest)
    {
        var order = new Order
        {
            OrderNumber = orderTableRequest.OrderNumber,
            SupplierOrderNumber = orderTableRequest.SupplierOrderNumber,
            SupplierId = orderTableRequest.SupplierId,
            VesselId = orderTableRequest.VesselId,
            WarehouseId = orderTableRequest.WarehouseId,
            ExpectedReadiness = orderTableRequest.ExpectedReadiness,
            ActualReadiness = orderTableRequest.ActualReadiness,
            ExpectedArrival = orderTableRequest.ExpectedArrival,
            ActualArrival = orderTableRequest.ActualArrival,
            OrderStatus = orderTableRequest.OrderStatus
        };
        await dbContext.Orders.AddAsync(order);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateOrder(int orderId, OrderRequest orderRequest)
    {
        if (!dbContext.Orders.Any(o => o.Id == orderId))
        {
            throw new KeyNotFoundException("Order not found");
        }

        if (!dbContext.Suppliers.Any(s => s.Id == orderRequest.SupplierId))
        {
            throw new KeyNotFoundException("Supplier not found");
        }

        if (!dbContext.Vessels.Any(v => v.Id == orderRequest.VesselId))
        {
            throw new KeyNotFoundException("Vessel not found");
        }

        if (!dbContext.Warehouses.Any(w => w.Id == orderRequest.WarehouseId))
        {
            throw new KeyNotFoundException("Warehouse not found");
        }

        var order = await dbContext.Orders.FindAsync(orderId);

        if (order == null)
        {
            throw new KeyNotFoundException("Order not found");
        }

        order.OrderNumber = orderRequest.OrderNumber;
        order.SupplierOrderNumber = orderRequest.SupplierOrderNumber;
        order.SupplierId = orderRequest.SupplierId;
        order.VesselId = orderRequest.VesselId;
        order.WarehouseId = orderRequest.WarehouseId;
        order.ExpectedReadiness = orderRequest.ExpectedReadiness;
        order.ActualReadiness = orderRequest.ActualReadiness;
        order.ExpectedArrival = orderRequest.ExpectedArrival;
        order.ActualArrival = orderRequest.ActualArrival;
        order.OrderStatus = orderRequest.OrderStatus;

        dbContext.Orders.Update(order);
        await dbContext.SaveChangesAsync();

        if (orderRequest.Boxes != null && orderRequest.Boxes.Count != 0)
        {
            var boxRequests = orderRequest.Boxes.Select(box => new BoxRequest
            {
                Length = box.Length,
                Width = box.Width,
                Height = box.Height,
                Weight = box.Weight
            }).ToList();

            await boxService.UpdateOrderBoxes(orderId, boxRequests);
        }
    }

    public async Task<List<string>?> GetAllOrderStatusesAsync()
        {
            if (memory.TryGetValue(OrderStatusCacheKey, out List<string>? cachedStatuses)) return cachedStatuses;
            cachedStatuses = await dbContext.OrderStatus.Select(s => s.Status).ToListAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(24));

            memory.Set(OrderStatusCacheKey, cachedStatuses, cacheEntryOptions);

            return cachedStatuses;
        }

        public async Task<OrderResponse> GetOrderById(int orderId)
        {
            var order = await dbContext.Orders
                .Include(o => o.Supplier)
                .Include(o => o.Vessel)
                .ThenInclude(v => v.Owner)
                .Include(o => o.Warehouse)
                .ThenInclude(a => a.Agent)
                .FirstOrDefaultAsync(o => o.Id == orderId) ?? throw new KeyNotFoundException("Order not found");

            var orderBoxes = await boxService.GetBoxes(orderId);

            return new OrderResponse
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                SupplierOrderNumber = order.SupplierOrderNumber,
                ExpectedReadiness = order.ExpectedReadiness,
                ActualReadiness = order.ActualReadiness,
                ExpectedArrival = order.ExpectedArrival,
                ActualArrival = order.ActualArrival,
                Supplier = new SupplierResponse
                {
                    Id = order.Supplier.Id,
                    Name = order.Supplier.Name
                },
                Owner = new OwnerResponse
                {
                    Id = order.Vessel.Owner.Id,
                    Name = order.Vessel.Owner.Name
                },
                Vessel = new VesselResponse
                {
                    Id = order.Vessel.Id,
                    Name = order.Vessel.Name,
                    ImoNumber = order.Vessel.ImoNumber,
                    Flag = order.Vessel.Flag
                },
                Agent = new AgentResponse
                {
                    Id = order.Warehouse.Agent.Id,
                    Name = order.Warehouse.Agent.Name
                },
                Warehouse = new WarehouseResponse
                {
                    Id = order.Warehouse.Id,
                    Name = order.Warehouse.Name
                },
                OrderStatus = order.OrderStatus,
                Boxes = orderBoxes.FirstOrDefault()?.Boxes
            };
        }

        public void DeleteOrder(int orderId)
        {
            var order = dbContext.Orders.Find(orderId) ?? throw new KeyNotFoundException("Order not found");

            dbContext.Orders.Remove(order);
            dbContext.SaveChanges();
        }
}
