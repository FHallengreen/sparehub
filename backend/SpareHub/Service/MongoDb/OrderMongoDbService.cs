using AutoMapper;
using Domain.Models;
using Repository.Interfaces;
using Repository.MongoDb;
using Service.Interfaces;
using Shared;
using Shared.DTOs.Order;
using Shared.Order;

namespace Service.MongoDb;

public class OrderMongoDbService (OrderMongoDbRepository orderRepository, IMapper mapper) : IOrderService
{
    public async Task<IEnumerable<OrderTableResponse>> GetOrders(List<string>? searchTerms)
    {
        var availableStatuses = await GetAllOrderStatusesAsync();

        IEnumerable<Order> orders;

        if (searchTerms != null && searchTerms.Any(term => availableStatuses.Contains(term, StringComparer.OrdinalIgnoreCase)))
        {
            orders = (await orderRepository.GetOrdersAsync())
                .Where(o => searchTerms.Contains(o.OrderStatus, StringComparer.OrdinalIgnoreCase));
        }
        else
        {
            orders = await orderRepository.GetNonCancelledOrdersAsync();
        }

        if (!orders.Any())
            throw new KeyNotFoundException("No orders found matching the search criteria.");

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

    public Task<OrderResponse> CreateOrder(OrderRequest orderRequest)
    {
        throw new NotImplementedException();
    }

    public Task UpdateOrder(string orderId, OrderRequest orderRequest)
    {
        throw new NotImplementedException();
    }

    public Task<OrderResponse> GetOrderById(string orderId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteOrder(string orderId)
    {
        throw new NotImplementedException();
    }

    public Task<List<string>> GetAllOrderStatusesAsync()
    {
        var statuses = orderRepository.GetAllOrderStatusesAsync();
        return statuses;
    }
}