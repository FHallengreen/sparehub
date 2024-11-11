using Shared;

namespace Service.Supplier;

public interface ISupplierService
{
    Task<List<SupplierResponse>> GetSuppliersBySearchQuery(string? searchQuery);
}
