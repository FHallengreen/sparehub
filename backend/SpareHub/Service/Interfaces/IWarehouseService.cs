using Shared;
using Shared.DTOs.Warehouse;

namespace Service.Interfaces;

public interface IWarehouseService
{
    Task<List<WarehouseResponse>> GetWarehousesBySearchQuery(string? searchQuery);
}
