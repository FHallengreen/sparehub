using Shared;

namespace Service.Warehouse;

public interface IWarehouseService
{
    Task<List<WarehouseResponse>> GetWarehousesBySearchQuery(string? searchQuery);
    Task<WarehouseResponse> GetWarehouseById(string warehouseId);
    Task<WarehouseResponse> CreateWarehouse(WarehouseRequest request);

}
