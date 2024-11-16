using Shared;

namespace Service.Warehouse;

public interface IWarehouseService
{
    Task<List<WarehouseResponse>> GetWarehousesBySearchQuery(string? searchQuery);
}
