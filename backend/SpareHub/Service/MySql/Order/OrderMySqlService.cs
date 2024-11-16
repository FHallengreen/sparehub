using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Persistence;
using Shared;
using Shared.Order;

namespace Service.Order;

public class OrderMySqlService(
    SpareHubDbContext dbContext,
    IMemoryCache memory,
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
            .Include(o => o.Boxes)
            .Select(o => new
            {
                Order = o,
                BoxesCount = o.Boxes.Count,
                TotalWeight = o.Boxes.Sum(b => b.Weight)
            });

        var orderStatusFilter = searchTerms?.FirstOrDefault(term =>
            availableStatuses != null && availableStatuses.Contains(term, StringComparer.OrdinalIgnoreCase));

        query = orderStatusFilter != null
            ? query.Where(o => o.Order.OrderStatus.Equals(orderStatusFilter, StringComparison.OrdinalIgnoreCase))
            : query.Where(o => !o.Order.OrderStatus.Equals("Cancelled", StringComparison.OrdinalIgnoreCase));

        var orders = await query.ToListAsync();

        var orderResponses = orders.Select(o => new OrderTableResponse
        {
            Id = o.Order.Id,
            OrderNumber = o.Order.OrderNumber,
            SupplierName = o.Order.Supplier.Name,
            OwnerName = o.Order.Vessel.Owner.Name,
            VesselName = o.Order.Vessel.Name,
            WarehouseName = o.Order.Warehouse.Name,
            OrderStatus = o.Order.OrderStatus,
            Boxes = o.BoxesCount,
            TotalWeight = o.TotalWeight
        }).ToList();

        if (searchTerms is not { Count: > 0 }) return orderResponses;

        var nonStatusTerms = searchTerms.Where(t =>
            availableStatuses != null && !availableStatuses.Contains(t, StringComparer.OrdinalIgnoreCase)).ToList();

        return nonStatusTerms.Aggregate(orderResponses, (current, term) => current.Where(o =>
            o.WarehouseName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
            o.VesselName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
            o.OrderNumber.Contains(term, StringComparison.OrdinalIgnoreCase) ||
            o.SupplierName.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList());
    }

    public async Task<OrderResponse> CreateOrder(OrderRequest orderTableRequest)
{
    // Create a new Order entity
    var order = new Domain.Order
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

    var supplier = await dbContext.Suppliers.FindAsync(order.SupplierId)
        ?? throw new KeyNotFoundException($"Supplier with ID {order.SupplierId} not found");

    var vessel = await dbContext.Vessels
        .Include(v => v.Owner)
        .FirstOrDefaultAsync(v => v.Id == order.VesselId)
        ?? throw new KeyNotFoundException($"Vessel with ID {order.VesselId} not found");

    var warehouse = await dbContext.Warehouses.FindAsync(order.WarehouseId)
        ?? throw new KeyNotFoundException($"Warehouse with ID {order.WarehouseId} not found");

    return new OrderResponse
    {
        Id = order.Id,
        OrderNumber = order.OrderNumber,
        SupplierOrderNumber = order.SupplierOrderNumber,
        ExpectedReadiness = order.ExpectedReadiness,
        ActualReadiness = order.ActualReadiness,
        ExpectedArrival = order.ExpectedArrival,
        ActualArrival = order.ActualArrival,
        OrderStatus = order.OrderStatus,
        Supplier = new SupplierResponse
        {
            Id = supplier.Id,
            Name = supplier.Name
        },
        Vessel = new VesselResponse
        {
            Id = vessel.Id,
            Name = vessel.Name,
            ImoNumber = vessel.ImoNumber,
            Flag = vessel.Flag,
            Owner = new OwnerResponse
            {
                Id = vessel.Owner.Id,
                Name = vessel.Owner.Name
            }
        },
        Warehouse = new WarehouseResponse
        {
            Id = warehouse.Id,
            Name = warehouse.Name
        },
        Owner = vessel.Owner.Name
    };
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
                BoxId = box.Id,
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
                        .ThenInclude(w => w.Agent)
                        .FirstOrDefaultAsync(o => o.Id == orderId)
                    ?? throw new KeyNotFoundException("Order not found");

        var orderBoxes = await boxService.GetBoxes(orderId);

        var boxes = orderBoxes.FirstOrDefault()?.Boxes?
            .Select(b => new BoxResponse
            {
                Id = b.Id,
                Length = b.Length,
                Width = b.Width,
                Height = b.Height,
                Weight = b.Weight
            }).ToList() ?? new List<BoxResponse>();


        return new OrderResponse
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            SupplierOrderNumber = order.SupplierOrderNumber,
            ExpectedReadiness = order.ExpectedReadiness,
            ActualReadiness = order.ActualReadiness,
            ExpectedArrival = order.ExpectedArrival,
            ActualArrival = order.ActualArrival,
            OrderStatus = order.OrderStatus,
            Supplier = new SupplierResponse
            {
                Id = order.Supplier.Id,
                Name = order.Supplier.Name
            },
            Vessel = new VesselResponse
            {
                Id = order.Vessel.Id,
                Name = order.Vessel.Name,
                ImoNumber = order.Vessel.ImoNumber,
                Flag = order.Vessel.Flag,
                Owner = new OwnerResponse
                {
                    Id = order.Vessel.Owner.Id,
                    Name = order.Vessel.Owner.Name
                }
            },
            Owner = order.Vessel.Owner.Name,
            Warehouse = new WarehouseResponse
            {
                Id = order.Warehouse.Id,
                Name = order.Warehouse.Name,
            },
            Agent = new AgentResponse
            {
                Id = order.Warehouse.Agent.Id,
                Name = order.Warehouse.Agent.Name
            },
            Boxes = boxes
        };
    }

    public void DeleteOrder(int orderId)
    {
        var order = dbContext.Orders.Find(orderId) ?? throw new KeyNotFoundException("Order not found");

        dbContext.Orders.Remove(order);
        dbContext.SaveChanges();
    }
}
