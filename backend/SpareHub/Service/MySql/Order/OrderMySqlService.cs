using Domain.Models;
using Microsoft.Extensions.Caching.Memory;
using Repository.Interfaces;
using Service.Interfaces;
using Shared;
using Shared.Order;

namespace Service.MySql.Order;

public class OrderMySqlService(IOrderRepository orderRepository, IMemoryCache memoryCache, IBoxService boxService)
    : IOrderService
{
    private const string OrderStatusCacheKey = "OrderStatuses";

    public async Task<IEnumerable<OrderTableResponse>> GetOrders(List<string>? searchTerms = null)
    {
        var availableStatuses = await GetAllOrderStatusesAsync();

        var orders = await orderRepository.GetOrdersAsync();

        var orderResponses = orders.Select(o => new OrderTableResponse
        {
            Id = o.Id,
            OrderNumber = o.OrderNumber,
            SupplierName = o.Supplier.Name,
            OwnerName = o.Vessel.Owner.Name,
            VesselName = o.Vessel.Name,
            WarehouseName = o.Warehouse.Name,
            OrderStatus = o.OrderStatus,
            Boxes = o.Boxes.Count,
            TotalWeight = o.Boxes.Sum(b => b.Weight)
        }).ToList();

        if (searchTerms is not { Count: > 0 })
            return orderResponses;

        var orderStatusFilter = searchTerms.FirstOrDefault(term =>
            availableStatuses != null && availableStatuses.Contains(term, StringComparer.OrdinalIgnoreCase));

        if (orderStatusFilter != null)
        {
            orderResponses = orderResponses
                .Where(o => o.OrderStatus.Equals(orderStatusFilter, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        else
        {
            orderResponses = orderResponses
                .Where(o => !o.OrderStatus.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        var nonStatusTerms = searchTerms.Where(t =>
            availableStatuses != null && !availableStatuses.Contains(t, StringComparer.OrdinalIgnoreCase)).ToList();

        return nonStatusTerms.Aggregate(orderResponses, (current, term) => current.Where(o =>
            o.WarehouseName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
            o.VesselName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
            o.OrderNumber.Contains(term, StringComparison.OrdinalIgnoreCase) ||
            o.SupplierName.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList());
    }

    public async Task<OrderResponse> CreateOrder(OrderRequest orderRequest)
    {
        var order = new Domain.Models.Order
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
            OrderStatus = orderRequest.OrderStatus,
            Boxes = new List<Box>()
        };

        await orderRepository.CreateOrderAsync(order);

        if (orderRequest.Boxes != null && orderRequest.Boxes.Any())
        {
            var boxRequests = orderRequest.Boxes.Select(b => new BoxRequest
            {
                Id = b.Id,
                Length = b.Length,
                Width = b.Width,
                Height = b.Height,
                Weight = b.Weight
            }).ToList();

            await boxService.UpdateOrderBoxes(order.Id, boxRequests);
        }

        var orderResponse = await GetOrderById(order.Id);
        return orderResponse;
    }

    public async Task UpdateOrder(string orderId, OrderRequest orderRequest)
    {
        var existingOrder = await orderRepository.GetOrderByIdAsync(orderId);
        if (existingOrder == null)
            throw new KeyNotFoundException("Order not found");

        // Update properties
        existingOrder.OrderNumber = orderRequest.OrderNumber;
        existingOrder.SupplierOrderNumber = orderRequest.SupplierOrderNumber;
        existingOrder.SupplierId = orderRequest.SupplierId;
        existingOrder.VesselId = orderRequest.VesselId;
        existingOrder.WarehouseId = orderRequest.WarehouseId;
        existingOrder.ExpectedReadiness = orderRequest.ExpectedReadiness;
        existingOrder.ActualReadiness = orderRequest.ActualReadiness;
        existingOrder.ExpectedArrival = orderRequest.ExpectedArrival;
        existingOrder.ActualArrival = orderRequest.ActualArrival;
        existingOrder.OrderStatus = orderRequest.OrderStatus;

        await orderRepository.UpdateOrderAsync(existingOrder);

        if (orderRequest.Boxes != null)
        {
            await boxService.UpdateOrderBoxes(orderId, orderRequest.Boxes);
        }
    }

    public async Task<OrderResponse> GetOrderById(string orderId)
    {
        var order = await orderRepository.GetOrderByIdAsync(orderId);
        if (order == null)
            throw new KeyNotFoundException("Order not found");

        var boxes = await boxService.GetBoxes(orderId);

        var orderResponse = new OrderResponse
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
            Warehouse = new WarehouseResponse
            {
                Id = order.Warehouse.Id,
                Name = order.Warehouse.Name,
                Agent = order.Warehouse.Agent != null ? new AgentResponse
                {
                    Id = order.Warehouse.Agent.Id,
                    Name = order.Warehouse.Agent.Name
                } : null
            },
            Owner = order.Vessel.Owner.Name,
            Boxes = boxes
        };

        return orderResponse;
    }


    public async Task DeleteOrder(string orderId)
    {
        await orderRepository.DeleteOrderAsync(orderId);
    }

    public async Task<List<string>> GetAllOrderStatusesAsync()
    {
        if (memoryCache.TryGetValue(OrderStatusCacheKey, out List<string>? cachedStatuses))
            return cachedStatuses!;

        cachedStatuses = await orderRepository.GetAllOrderStatusesAsync();

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromHours(24));

        memoryCache.Set(OrderStatusCacheKey, cachedStatuses, cacheEntryOptions);

        return cachedStatuses;
    }
}
