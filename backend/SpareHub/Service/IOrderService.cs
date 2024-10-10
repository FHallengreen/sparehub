using Domain;
using Shared;

namespace Service;

public interface IOrderService
{
    Task<IEnumerable<OrderResponse>> GetOrders(List<string>? search);
    Task CreateOrder(OrderRequest orderRequest);
    Task<List<string>?> GetAllOrderStatusesAsync();
}
