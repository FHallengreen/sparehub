using Domain.Models;

namespace Repository.Interfaces;

public interface IBoxRepository
{
    Task<Box> CreateBoxAsync(Box box);
    Task<List<Box>> GetBoxesByOrderIdAsync(string orderId);
    Task UpdateBoxesAsync(string orderId, List<Box> boxes);
    Task DeleteBoxAsync(string orderId, string boxId);
}