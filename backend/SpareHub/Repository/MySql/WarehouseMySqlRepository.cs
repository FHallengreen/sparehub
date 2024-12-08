using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.MySql;
using Persistence.MySql.SparehubDbContext;
using Repository.Interfaces;
using Shared.Exceptions;

namespace Repository.MySql;

public class WarehouseMySqlRepository(SpareHubDbContext dbContext, IMapper mapper) : IWarehouseRepository
{
    public async Task<List<Warehouse>> GetWarehousesAsync()
    {
        var warehouses = await dbContext.Warehouses
            .Include(w => w.Address)
            .Include(w => w.Agent)
            .ToListAsync();
        return mapper.Map<List<Warehouse>>(warehouses);
    }
    public async Task<List<Warehouse>> GetWarehousesBySearchQueryAsync(string? searchQuery)
    {
        return await dbContext.Warehouses
            .Where(w => string.IsNullOrEmpty(searchQuery) || w.Name.StartsWith(searchQuery))
            .Include(w => w.Address) 
            .Join(dbContext.Agents,
                warehouse => warehouse.AgentId,
                agent => agent.Id,
                (warehouse, agent) => new
                {
                    Warehouse = warehouse,
                    Agent = agent
                })
            .Select(result => new Warehouse
            {
                Id = result.Warehouse.Id.ToString(),
                Name = result.Warehouse.Name,
                Agent = new Agent
                {
                    Id = result.Agent.Id.ToString(),
                    Name = result.Agent.Name
                },
                Address = new Address
                {
                    Id = result.Warehouse.Address.Id.ToString(),
                    AddressLine = result.Warehouse.Address.AddressLine,
                    PostalCode = result.Warehouse.Address.PostalCode,
                    Country = result.Warehouse.Address.Country
                }
            })
            .ToListAsync();
    }


    public async Task<Warehouse> GetWarehouseByIdAsync(string warehouseId)
    {
        int parsedWarehouseId;
        if (!int.TryParse(warehouseId, out parsedWarehouseId))
            throw new ArgumentException("Invalid warehouse ID format.");

        var warehouseEntity = await dbContext.Warehouses
            .AsNoTracking()
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
        await dbContext.Warehouses.AddAsync(warehouseEntity);
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
            throw new NotFoundException("Warehouse not found");

        dbContext.Warehouses.Remove(warehouseEntity);
        await dbContext.SaveChangesAsync();
    }
}