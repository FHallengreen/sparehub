using Domain.Models;

namespace Repository.Interfaces;

public interface IWarehouseRepository
{   
    Task<List<Warehouse> >GetWarehousesAsync();
    Task<List<Warehouse>> GetWarehousesBySearchQueryAsync(string? searchQuery);
    Task<Warehouse> GetWarehouseByIdAsync(string warehouseId);
    Task<Warehouse>CreateWarehouseAsync(Warehouse warehouse);
    Task<Warehouse> UpdateWarehouseAsync(Warehouse warehouse);
    Task DeleteWarehouseAsync(string warehouseId);
}
