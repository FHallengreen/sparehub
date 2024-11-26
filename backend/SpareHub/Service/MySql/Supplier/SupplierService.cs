using Microsoft.EntityFrameworkCore;
using Persistence.MySql.SparehubDbContext;
using Service.Interfaces;
using Shared;
using Shared.DTOs.Supplier;

namespace Service.MySql.Supplier;

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
