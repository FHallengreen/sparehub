using Domain.Models;

namespace Repository.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task<Order?> GetOrderByIdAsync(string orderId);
    Task<IEnumerable<Order>> GetActiveOrders();
    Task CreateOrderAsync(Order order);
    Task UpdateOrderAsync(Order order);
    Task DeleteOrderAsync(string orderId);
    Task<List<string>> GetAllOrderStatusesAsync();
    Task<List<Order>> GetOrdersByIdsAsync(List<string> orderIds);
}
