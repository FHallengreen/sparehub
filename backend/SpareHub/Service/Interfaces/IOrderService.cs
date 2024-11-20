using Shared;
using Shared.DTOs.Order;
using Shared.Order;

namespace Service.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<OrderTableResponse>> GetOrders(List<string>? searchTerms);
    Task<OrderResponse> CreateOrder(OrderRequest orderRequest);
    Task UpdateOrder(string orderId, OrderRequest orderRequest);
    Task<OrderResponse> GetOrderById(string orderId);
    Task DeleteOrder(string orderId);
    Task<List<string>> GetAllOrderStatusesAsync();
}
