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
        Console.Out.WriteLine("Warehouse ID: " + warehouseId);
        if (!int.TryParse(warehouseId, out var parsedWarehouseId))
            throw new ArgumentException("Invalid warehouse ID format.");

        var warehouseEntity = await dbContext.Warehouses
            .Include(w => w.Agent)
            .Include(w => w.Address)
            .FirstOrDefaultAsync(w => w.Id == parsedWarehouseId);

        if (warehouseEntity == null)
            throw new KeyNotFoundException($"Warehouse with ID {warehouseId} not found.");

        // Assuming the mapper maps entities to the `Warehouse` domain model correctly.
        var warehouse = mapper.Map<Warehouse>(warehouseEntity);

        return warehouse;
    }


    public async Task<Warehouse> CreateWarehouseAsync(Warehouse warehouse)
    {   
        var warehouseEntity = mapper.Map<WarehouseEntity>(warehouse);
        dbContext.Warehouses.Add(warehouseEntity);
        await dbContext.SaveChangesAsync();
        warehouse.Id = warehouseEntity.Id.ToString();
        
        return warehouse;
    }

    public async Task<Warehouse> UpdateWarehouseAsync(Warehouse warehouse)
    {
        var warehouseEntity = mapper.Map<WarehouseEntity>(warehouse);
        dbContext.Warehouses.Update(warehouseEntity);
        await dbContext.SaveChangesAsync();
        return mapper.Map<Warehouse>(warehouseEntity);
    }

    public async Task DeleteWarehouseAsync(string warehouseId)
    {
        if (!int.TryParse(warehouseId, out var id))
            return;

        var warehouseEntity = await dbContext.Warehouses
            .FirstOrDefaultAsync(d => d.Id == id);

        if (warehouseEntity == null)
            return;

        dbContext.Warehouses.Remove(warehouseEntity);
        await dbContext.SaveChangesAsync();
    }
}