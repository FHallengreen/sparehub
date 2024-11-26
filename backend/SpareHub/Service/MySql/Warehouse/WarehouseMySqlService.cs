using System.ComponentModel.DataAnnotations;
using Repository.MySql;
using Service.Interfaces;
using Shared;
using Shared.DTOs.Address;
using Shared.Exceptions;

namespace Service.MySql.Warehouse;

public class WarehouseMySqlService(WarehouseMySqlRepository warehouseRepo, 
                              AddressMySqlRepository addressRepo, 
                              AgentMySqlRepository agentRepo) : IWarehouseService
{
    public async Task<List<WarehouseResponse>> GetWarehousesBySearchQuery(string? searchQuery)
    {
        var warehouses = await warehouseRepo.GetWarehousesBySearchQueryAsync(searchQuery);
        if (warehouses.Count == 0)
        {
            throw new NotFoundException("No warehouses found matching the search criteria.");
        }
        return warehouses.Select(w => new WarehouseResponse
        {
            Id = w.Id.ToString(),
            Name = w.Name,
            Address = new AddressResponse()
            {
                Id = w.Address.Id.ToString(),
                AddressLine = w.Address.AddressLine,
                PostalCode = w.Address.PostalCode,
                Country = w.Address.Country
            }
        }).ToList();
    }

    public async Task<WarehouseResponse> GetWarehouseById(string id)
    {
        var foundWarehouse = await warehouseRepo.GetWarehouseByIdAsync(id);
        if (foundWarehouse == null)
        {
            throw new NotFoundException($"No warehouse found with id {id}");
        }
        
        return new WarehouseResponse
        {
            Id = foundWarehouse.Id,
            Name = foundWarehouse.Name,
            Address = new AddressResponse
            {
                Id = foundWarehouse.Address.Id,
                AddressLine = foundWarehouse.Address.AddressLine,
                PostalCode = foundWarehouse.Address.PostalCode,
                Country = foundWarehouse.Address.Country
            }
        };
    }

    public async Task<WarehouseResponse> CreateWarehouse(WarehouseRequest request) {
        
        var address = await addressRepo.GetAddressByIdAsync(request.AddressId);
        if (address == null)
        {
            throw new NotFoundException($"No address found with id {request.AddressId}");
        }

        Domain.Models.Agent agent = null!;
        if (request.AgentId != null)
        {
            agent = await agentRepo.GetAgentByIdAsync(request.AgentId);
            if (agent == null)
            {
                throw new NotFoundException($"No agent found with id {request.AgentId}");
            }
        }
        
        var warehouse = new Domain.Models.Warehouse
        {
            Name = request.Name,
            Address = address,
            Agent = agent
        };
        
        await warehouseRepo.CreateWarehouseAsync(warehouse);
        
        return new WarehouseResponse
        {
            Id = warehouse.Id,
            Name = warehouse.Name,
            Address = new AddressResponse
            {
                Id = warehouse.Address.Id,
                AddressLine = warehouse.Address.AddressLine,
                PostalCode = warehouse.Address.PostalCode,
                Country = warehouse.Address.Country
            }
        };
    }

    public async Task<WarehouseResponse> UpdateWarehouse(string warehouseId, WarehouseRequest request)
    {
        var foundWarehouse = await warehouseRepo.GetWarehouseByIdAsync(warehouseId);
        if (foundWarehouse == null)
        {
            throw new NotFoundException($"No warehouse found with id {warehouseId}");
        }
        
        var address = await addressRepo.GetAddressByIdAsync(request.AddressId);
        if (address == null)
        {
            throw new NotFoundException($"No address found with id {request.AddressId}");
        }
        
        Domain.Models.Agent agent = null!;
        if (request.AgentId != null)
        {
            agent = await agentRepo.GetAgentByIdAsync(request.AgentId);
            if (agent == null)
            {
                throw new NotFoundException($"No agent found with id {request.AgentId}");
            }
        }
        
        foundWarehouse.Name = request.Name;
        foundWarehouse.Address = address;
        foundWarehouse.Agent = agent;
        
        await warehouseRepo.UpdateWarehouseAsync(foundWarehouse);
        
        return new WarehouseResponse()
        {
            Id = foundWarehouse.Id,
            Name = foundWarehouse.Name,
            Address = new AddressResponse
            {
                Id = foundWarehouse.Address.Id,
                AddressLine = foundWarehouse.Address.AddressLine,
                PostalCode = foundWarehouse.Address.PostalCode,
                Country = foundWarehouse.Address.Country
            }
        };
    }

    public async Task DeleteWarehouse(string warehouseId)
    {
        if (string.IsNullOrWhiteSpace(warehouseId))
            throw new ValidationException("Warehouse ID cannot be null or empty.");

        await warehouseRepo.DeleteWarehouseAsync(warehouseId);
    }
}
