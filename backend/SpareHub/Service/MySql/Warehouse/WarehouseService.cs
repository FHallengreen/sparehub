using Microsoft.EntityFrameworkCore;
using Persistence;
using Shared;
using Shared.DTOs.Order;

namespace Service.Warehouse;

public class WarehouseService(SpareHubDbContext dbContext) : IWarehouseService
{
    public async Task<List<WarehouseResponse>> GetWarehousesBySearchQuery(string? searchQuery)
    {
        return await dbContext.Warehouses
            .Where(w => string.IsNullOrEmpty(searchQuery) || w.Name.StartsWith(searchQuery))
            .Join(dbContext.Agents,
                warehouse => warehouse.AgentId,
                agent => agent.Id,
                (warehouse, agent) => new WarehouseResponse
                {
                    Id = warehouse.Id.ToString(),
                    Name = warehouse.Name,
                    Agent = new AgentResponse
                    {
                        Id = agent.Id.ToString(),
                        Name = agent.Name
                    }
                })
            .ToListAsync();
    }
}
