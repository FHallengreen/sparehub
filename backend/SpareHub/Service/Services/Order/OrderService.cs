using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Repository.Interfaces;
using Service.Interfaces;
using Shared.DTOs.Order;
using Shared.DTOs.Owner;
using Shared.DTOs.Supplier;
using Shared.DTOs.Vessel;
using Shared.DTOs.Warehouse;
using Shared.Exceptions;


namespace Service.Services.Order;

public class OrderService(
    IOrderRepository orderRepository,
    IMemoryCache memoryCache,
    IBoxService boxService,
    IMapper mapper)
    : IOrderService
{
    private const string OrderStatusCacheKey = "OrderStatuses";

    public async Task<IEnumerable<OrderTableResponse>> GetOrders(List<string>? searchTerms = null)
    {
        var availableStatuses = await GetAllOrderStatusesAsync();
        var orders = await FetchOrdersBasedOnSearchTerms(searchTerms, availableStatuses);

        var enumerable = orders.ToList();
        if (enumerable.Count == 0)
        {
            throw new NotFoundException("No orders found matching the search criteria.");
        }

        var orderResponses = MapOrdersToResponses(enumerable);

        if (searchTerms is { Count: > 0 })
        {
            orderResponses = FilterOrderResponses(orderResponses, searchTerms, availableStatuses);
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

        orderRequest.Transporter =
            string.IsNullOrWhiteSpace(orderRequest.Transporter) ? null : orderRequest.Transporter;

        mapper.Map(orderRequest, existingOrder);
        await orderRepository.UpdateOrderAsync(existingOrder);

        if (orderRequest.Boxes != null)
        {
            var boxesToUpdate = orderRequest.Boxes.Where(b => !string.IsNullOrWhiteSpace(b.Id)).ToList();
            var boxesToCreate = orderRequest.Boxes.Where(b => string.IsNullOrWhiteSpace(b.Id)).ToList();

            if (boxesToUpdate.Any())
            {
                await boxService.UpdateBoxes(orderId, boxesToUpdate);
            }

            if (boxesToCreate.Any())
            {
                foreach (var newBox in boxesToCreate)
                {
                    await boxService.CreateBox(newBox, orderId);
                }
            }
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
            TrackingNumber = order.TrackingNumber,
            Transporter = order.Transporter,
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
        {
            if (cachedStatuses == null || cachedStatuses.Count == 0)
            {
                cachedStatuses = await FetchAndCacheStatusesAsync();
            }

            return cachedStatuses;
        }

        return await FetchAndCacheStatusesAsync();
    }

    private async Task<List<string>> FetchAndCacheStatusesAsync()
    {
        var statuses = await orderRepository.GetAllOrderStatusesAsync();

        if (statuses.Count != 0)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(24));

            memoryCache.Set(OrderStatusCacheKey, statuses, cacheEntryOptions);
        }

        return statuses;
    }

    private static List<OrderTableResponse> MapOrdersToResponses(IEnumerable<Domain.Models.Order> orders)
    {
        return orders.Select(o => new OrderTableResponse
        {
            Id = o.Id,
            OrderNumber = o.OrderNumber,
            SupplierName = o.Supplier?.Name ?? "Unknown Supplier",
            OwnerName = o.Vessel?.Owner?.Name ?? "Unknown Owner",
            VesselName = o.Vessel?.Name ?? "Unknown Vessel",
            WarehouseName = o.Warehouse?.Name ?? "Unknown Warehouse",
            OrderStatus = o.OrderStatus,
            Boxes = o.Boxes?.Count ?? 0,
            TotalWeight = o.Boxes?.Sum(b => b.Weight) ?? 0,
            TotalVolume = o.Boxes?.Sum(b => b.Length * b.Width * b.Height / 1_000_000.0) ?? 0,
            TotalVolumetricWeight = o.Boxes?.Sum(b => b.Length * b.Width * b.Height / 6000.0) ?? 0
        }).ToList();
    }


    private static List<OrderTableResponse> FilterOrderResponses(
        List<OrderTableResponse> orderResponses, List<string> searchTerms, IEnumerable<string> availableStatuses)
    {
        var nonStatusTerms = searchTerms
            .Where(t => !availableStatuses.Contains(t, StringComparer.OrdinalIgnoreCase))
            .ToList();

        return nonStatusTerms.Aggregate(orderResponses, (current, term) => current.Where(o =>
            o.WarehouseName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
            o.VesselName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
            o.OrderNumber.Contains(term, StringComparison.OrdinalIgnoreCase) ||
            o.SupplierName.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList());
    }

    private async Task<IEnumerable<Domain.Models.Order>> FetchOrdersBasedOnSearchTerms(
        List<string>? searchTerms, IEnumerable<string> availableStatuses)
    {
        if (searchTerms != null &&
            searchTerms.Any(term => availableStatuses.Contains(term, StringComparer.OrdinalIgnoreCase)))
        {
            return (await orderRepository.GetOrdersAsync())
                .Where(o => searchTerms.Contains(o.OrderStatus, StringComparer.OrdinalIgnoreCase));
        }

        return await orderRepository.GetNotActiveOrders();
    }
}