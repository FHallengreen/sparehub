using Domain;
using Shared;

namespace Service;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetOrders();
    Task CreateOrder(OrderRequest orderRequest);
}
