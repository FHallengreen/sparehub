using Microsoft.EntityFrameworkCore;
using Persistence;
using Shared;

namespace Service.Warehouse;

public class WarehouseService(SpareHubDbContext dbContext) : IWarehouseService
{
    public async Task<List<WarehouseResponse>> GetWarehousesBySearchQuery(string? searchQuery)
    {
        return await dbContext.Warehouses
            .Where(w => string.IsNullOrEmpty(searchQuery) || w.Name.StartsWith(searchQuery))
            .Include(w => w.Agent)
            .Select(w => new WarehouseResponse
            {
                Id = w.Id,
                Name = w.Name,
                Agent = new AgentResponse
                {
                    Id = w.Agent.Id,
                    Name = w.Agent.Name
                }
            })
            .ToListAsync();
    }
}
