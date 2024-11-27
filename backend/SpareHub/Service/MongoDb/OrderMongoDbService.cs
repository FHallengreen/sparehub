using AutoMapper;
using Domain.Models;
using Persistence.MongoDb;
using Repository.Interfaces;
using Repository.MongoDb;
using Service.Interfaces;
using Shared;
using Shared.DTOs.Order;
using Shared.Exceptions;
using Shared.Order;

namespace Service.MongoDb;

public class OrderMongoDbService(
    OrderMongoDbRepository orderRepository,
    BoxMongoDbRepository boxRepository,
    IMapper mapper) : IOrderService
{
    public async Task<IEnumerable<OrderTableResponse>> GetOrders(List<string>? searchTerms)
    {
        var availableStatuses = await GetAllOrderStatusesAsync();

        IEnumerable<Order> orders;

        if (searchTerms != null &&
            searchTerms.Any(term => availableStatuses.Contains(term, StringComparer.OrdinalIgnoreCase)))
        {
            orders = (await orderRepository.GetOrdersAsync())
                .Where(o => searchTerms.Contains(o.OrderStatus.ToString(), StringComparer.OrdinalIgnoreCase));
        }
        else
        {
            orders = await orderRepository.GetNonCancelledOrdersAsync();
        }

        if (!orders.Any())
            throw new NotFoundException("No orders found matching the search criteria.");

        var orderIds = orders.Select(o => o.Id).ToList();
        var boxCollections = await GetBoxesByOrderIdsAsync(orderIds);
        var boxLookup = boxCollections.ToLookup(b => b.OrderId);

        var orderResponses = orders.Select(order =>
        {
            var orderBoxes = boxLookup[order.Id].ToList();
            return new OrderTableResponse
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                SupplierName = order.Supplier.Name, // Use flat field
                OwnerName = order.Vessel.Owner.Name,
                VesselName = order.Vessel.Name,     // Use flat field
                WarehouseName = order.Warehouse.Name, // Use flat field
                OrderStatus = order.OrderStatus.ToString(),
                Boxes = orderBoxes.Count,
                TotalWeight = orderBoxes.Sum(b => b.Weight)
            };
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


    private async Task<List<Box>> GetBoxesByOrderIdsAsync(IEnumerable<string> orderIds)
    {
        var boxTasks = orderIds.Select(orderId => boxRepository.GetBoxesByOrderIdAsync(orderId));

        var boxesArray = await Task.WhenAll(boxTasks);

        return boxesArray.SelectMany(boxes => boxes).ToList();
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