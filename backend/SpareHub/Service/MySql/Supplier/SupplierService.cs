using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.MySql.SparehubDbContext;
using Shared;

namespace Service.Supplier;

public class SupplierService(SpareHubDbContext dbContext) : ISupplierService
{
    public async Task<List<SupplierResponse>> GetSuppliersBySearchQuery(string? searchQuery = "")
    {
        return await dbContext.Suppliers
            .Where(v => string.IsNullOrEmpty(searchQuery) || v.Name.StartsWith(searchQuery))
            .Select(s => new SupplierResponse
            {
                Id = s.Id.ToString(),
                Name = s.Name
            }).ToListAsync();
    }
}
