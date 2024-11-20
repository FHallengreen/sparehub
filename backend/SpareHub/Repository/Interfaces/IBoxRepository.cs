using Domain.Models;

namespace Repository.Interfaces;

public interface IBoxRepository
{
    Task<Box> CreateBoxAsync(Box box);
    Task<List<Box>> GetBoxesByOrderIdAsync(string orderId);
    Task UpdateBoxesAsync(string orderId, List<Box> boxRequests);
    Task DeleteBoxAsync(string orderId, string boxId);
    Task UpdateBoxAsync(string orderId, Box box);
}
