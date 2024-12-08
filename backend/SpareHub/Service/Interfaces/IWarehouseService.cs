using Shared;
using Shared.DTOs.Warehouse;

namespace Service.Interfaces;

public interface IWarehouseService
{
    Task<List<WarehouseResponse>> GetWarehouses();
    Task<List<WarehouseResponse>> GetWarehousesBySearchQuery(string? searchQuery);
    Task<WarehouseResponse> GetWarehouseById(string warehouseId);
    Task<WarehouseResponse> CreateWarehouse(WarehouseRequest request);
    
    Task<WarehouseResponse> UpdateWarehouse(string warehouseId, WarehouseRequest request);
    Task DeleteWarehouse(string warehouseId);

}
