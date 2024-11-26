using Shared;
using Shared.DTOs.Supplier;

namespace Service.Interfaces;

public interface ISupplierService
{
    Task<List<SupplierResponse>> GetSuppliersBySearchQuery(string? searchQuery);
}
