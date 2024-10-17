using Domain;
using Shared;

namespace Service;

public interface IOrderService
{
    Task<IEnumerable<OrderTableResponse>> GetOrders(List<string>? search);
    Task CreateOrder(OrderRequest orderTableRequest);
    Task UpdateOrder(int orderId, OrderRequest orderRequest);
    Task<List<string>?> GetAllOrderStatusesAsync();
    Task<OrderResponse> GetOrderById(int orderId);
}
