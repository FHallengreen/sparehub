using Domain.Models;
using Shared;

namespace Repository.Interfaces;

public interface IWarehouseRepository
{
    Task<List<Warehouse>> GetWarehousesBySearchQueryAsync(string? searchQuery);
    Task<Warehouse> GetWarehouseByIdAsync(string warehouseId);
    Task CreateWarehouseAsync(Warehouse warehouse);
}
