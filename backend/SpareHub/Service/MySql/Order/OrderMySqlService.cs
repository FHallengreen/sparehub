using Domain.Models;
using Microsoft.Extensions.Caching.Memory;
using Repository.Interfaces;
using Service.Interfaces;
using Shared;
using Shared.Exceptions;
using Shared.Order;

namespace Service.MySql.Order;

public class OrderMySqlService(IOrderRepository orderRepository, IMemoryCache memoryCache, IBoxService boxService)
    : IOrderService
{
    private const string OrderStatusCacheKey = "OrderStatuses";

    public async Task<IEnumerable<OrderTableResponse>> GetOrders(List<string>? searchTerms = null)
    {
        var availableStatuses = await GetAllOrderStatusesAsync();

        IEnumerable<Domain.Models.Order> orders;

        try
        {
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
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while fetching orders from the database.", ex);
        }

        if (orders == null || !orders.Any())
        {
            throw new KeyNotFoundException("No orders found matching the search criteria.");
        }

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

        if (searchTerms is { Count: > 0 })
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
        try
        {
            await orderRepository.CreateOrderAsync(order);
        }
        catch (Exception ex)
        {
            throw new RepositoryException("Failed to create order.", ex);
        }

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

            try
            {
                await boxService.UpdateOrderBoxes(order.Id, boxRequests);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Failed to create boxes for order.", ex);
            }
        }

        return await GetOrderById(order.Id);
    }

    public async Task UpdateOrder(string orderId, OrderRequest orderRequest)
    {
        var existingOrder = await orderRepository.GetOrderByIdAsync(orderId);
        if (existingOrder == null)
            throw new KeyNotFoundException("Order not found");

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

        try
        {
            await orderRepository.UpdateOrderAsync(existingOrder);
        }
        catch (Exception ex)
        {
            throw new RepositoryException("Failed to update order.", ex);
        }

        if (orderRequest.Boxes != null)
        {
            await boxService.UpdateOrderBoxes(orderId, orderRequest.Boxes);
        }
    }

    public async Task<OrderResponse> GetOrderById(string orderId)
    {
        try
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
        catch (Exception ex)
        {
            throw new RepositoryException("Failed to get order.", ex);
        }
    }


    public async Task DeleteOrder(string orderId)
    {
        try
        {
            await orderRepository.DeleteOrderAsync(orderId);
        }
        catch (KeyNotFoundException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while deleting the order.", ex);
        }
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