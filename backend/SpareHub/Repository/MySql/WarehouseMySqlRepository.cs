using AutoMapper;
using Domain.Models;
using Domain.MySql;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Repository.Interfaces;

namespace Repository.MySql;

public class WarehouseMySqlRepository(SpareHubDbContext dbContext, IMapper mapper) : IWarehouseRepository
{
    public async Task<List<Warehouse>> GetWarehousesBySearchQueryAsync(string? searchQuery)
    {
        return await dbContext.Warehouses
            .Where(w => string.IsNullOrEmpty(searchQuery) || w.Name.StartsWith(searchQuery))
            .Join(dbContext.Agents,
                warehouse => warehouse.AgentId,
                agent => agent.Id,
                (warehouse, agent) => new Warehouse
                {
                    Id = warehouse.Id.ToString(),
                    Name = warehouse.Name,
                    Agent = new Agent
                    {
                        Id = agent.Id.ToString(),
                        Name = agent.Name
                    }
                })
            .ToListAsync();
    }

    public async Task<Warehouse> GetWarehouseByIdAsync(string warehouseId)
    {
        var warehouses = await dbContext.Warehouses
            .Include(w => w.Agent)
            .Include(w => w.Address)
            .FirstOrDefaultAsync(w => w.Id == Int32.Parse(warehouseId));
        
        return await mapper.Map<Task<Warehouse>>(warehouses);
    }

    public async Task CreateWarehouseAsync(Warehouse warehouse)
    {   
        var warehouseEntity = mapper.Map<WarehouseEntity>(warehouse);
        dbContext.Warehouses.Add(warehouseEntity);
        await dbContext.SaveChangesAsync();
        warehouse.Id = warehouseEntity.Id.ToString();
    }
}