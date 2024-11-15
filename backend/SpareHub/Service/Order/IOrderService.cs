using Shared;

namespace Service.Order;

public interface IOrderService
{
    Task<IEnumerable<OrderTableResponse>> GetOrders(List<string>? search);
    Task<Domain.Order> CreateOrder(OrderRequest orderTableRequest);
    Task UpdateOrder(int orderId, OrderRequest orderRequest);
    Task<List<string>?> GetAllOrderStatusesAsync();
    Task<OrderResponse> GetOrderById(int orderId);
    void DeleteOrder(int orderId);
}
