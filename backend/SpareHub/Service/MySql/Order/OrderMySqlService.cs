using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Repository.MySql;
using Service.Interfaces;
using Shared;
using Shared.DTOs.Order;
using Shared.Exceptions;
using Shared.Order;

namespace Service.MySql.Order;

public class OrderMySqlService(
    OrderMySqlRepository orderRepository,
    IMemoryCache memoryCache,
    IBoxService boxService,
    IMapper mapper)
    : IOrderService
{
    private const string OrderStatusCacheKey = "OrderStatuses";

    public async Task<IEnumerable<OrderTableResponse>> GetOrders(List<string>? searchTerms = null)
    {
        var availableStatuses = await GetAllOrderStatusesAsync();

        IEnumerable<Domain.Models.Order> orders;


        if (searchTerms != null &&
            searchTerms.Any(term => availableStatuses.Contains(term, StringComparer.OrdinalIgnoreCase)))
        {
            orders = (await orderRepository.GetOrdersAsync())
                .Where(o => searchTerms.Contains(o.OrderStatus, StringComparer.OrdinalIgnoreCase));
        }
        else
        {
            orders = await orderRepository.GetNonCancelledOrdersAsync();
        }

        var enumerable = orders.ToList();
        if (orders == null || enumerable.Count == 0)
        {
            throw new NotFoundException("No orders found matching the search criteria.");
        }

        var orderResponses = enumerable.Select(o => new OrderTableResponse
        {
            Id = o.Id,
            OrderNumber = o.OrderNumber,
            SupplierName = o.Supplier.Name,
            OwnerName = o.Vessel.Owner.Name,
            VesselName = o.Vessel.Name,
            WarehouseName = o.Warehouse.Name,
            OrderStatus = o.OrderStatus,
            Boxes = o.Boxes.Count,
            TotalWeight = Math.Round(o.Boxes.Sum(b => b.Weight), 2)
        }).ToList();

        if (searchTerms is not { Count: > 0 }) return orderResponses;
        {
            var nonStatusTerms = searchTerms.Where(t =>
                !availableStatuses.Contains(t, StringComparer.OrdinalIgnoreCase)).ToList();

            orderResponses = nonStatusTerms.Aggregate(orderResponses, (current, term) => current.Where(o =>
                o.WarehouseName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                o.VesselName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                o.OrderNumber.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                o.SupplierName.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList());
        }

        return orderResponses;
    }

    public async Task<OrderResponse> CreateOrder(OrderRequest orderRequest)
    {
        var order = mapper.Map<Domain.Models.Order>(orderRequest);
        await orderRepository.CreateOrderAsync(order);

        if (orderRequest.Boxes == null || orderRequest.Boxes.Count == 0) return await GetOrderById(order.Id);
        {
            var boxRequests = orderRequest.Boxes.Select(b => new BoxRequest
            {
                Id = b.Id,
                Length = b.Length,
                Width = b.Width,
                Height = b.Height,
                Weight = b.Weight
            }).ToList();

            foreach (var box in boxRequests)
            {
                await boxService.CreateBox(box, order.Id);
            }

            return await GetOrderById(order.Id);
        }
    }

    public async Task UpdateOrder(string orderId, OrderRequest orderRequest)
    {
        var existingOrder = await orderRepository.GetOrderByIdAsync(orderId);
        if (existingOrder == null)
            throw new NotFoundException($"No order found with id {orderId}");

        mapper.Map(orderRequest, existingOrder);

        await orderRepository.UpdateOrderAsync(existingOrder);

        if (orderRequest.Boxes != null)
        {
            await boxService.UpdateBoxes(orderId, orderRequest.Boxes);
        }
    }

    public async Task<OrderResponse> GetOrderById(string orderId)
    {
        var order = await orderRepository.GetOrderByIdAsync(orderId);
        if (order == null)
            throw new NotFoundException($"No order found with id {orderId}");

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
                Agent = order.Warehouse.Agent != null
                    ? new AgentResponse
                    {
                        Id = order.Warehouse.Agent.Id,
                        Name = order.Warehouse.Agent.Name
                    }
                    : null
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