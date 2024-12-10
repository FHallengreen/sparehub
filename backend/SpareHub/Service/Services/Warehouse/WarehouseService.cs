using System.ComponentModel.DataAnnotations;
using Repository.Interfaces;
using Service.Interfaces;
using Shared.DTOs.Address;
using Shared.DTOs.Order;
using Shared.DTOs.Warehouse;
using Shared.Exceptions;

namespace Service.Services.Warehouse;

public class WarehouseService(IWarehouseRepository warehouseRepo, IAgentRepository agentRepo, IAddressRepository addressRepo) : IWarehouseService
{
    public async Task<List<WarehouseResponse>> GetWarehouses()
    {
        var warehouses = await warehouseRepo.GetWarehousesAsync();

        foreach (var w in warehouses)
        {
            //Print all values in warehouse
            Console.WriteLine(w.Id + ": " + w.Name);
            Console.WriteLine(w.Address);
            Console.WriteLine(w.Agent);
            
        }
        return warehouses.Select(w => new WarehouseResponse
        {
            Id = w.Id,
            Name = w.Name,
            Address = new AddressResponse
            {
                Id = w.Address.Id,
                AddressLine = w.Address.AddressLine,
                PostalCode = w.Address.PostalCode,
                Country = w.Address.Country
            }, // Handle null Address
            Agent = w.Agent != null ? new AgentResponse
            {
                Id = w.Agent.Id,
                Name = w.Agent.Name
            } : null // Handle null Agent
        }).ToList();
    }

    public async Task<List<WarehouseResponse>> GetWarehousesBySearchQuery(string? searchQuery)
    {   
        var warehouses = await warehouseRepo.GetWarehousesBySearchQueryAsync(searchQuery);
    
        return warehouses.Select(w => new WarehouseResponse
        {
            Id = w.Id,
            Name = w.Name,
            Address = new AddressResponse
            {
                Id = w.Address.Id,
                AddressLine = w.Address.AddressLine,
                PostalCode = w.Address.PostalCode,
                Country = w.Address.Country
            }, // Handle null Address
            Agent = w.Agent != null ? new AgentResponse
            {
                Id = w.Agent.Id,
                Name = w.Agent.Name
            } : null // Handle null Agent
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
            },
            Agent = new AgentResponse
            {
                Id = foundWarehouse.Agent?.Id,
                Name = foundWarehouse.Agent.Name
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
            },
            Agent = new AgentResponse
            {
                Id = warehouse.Agent.Id,
                Name = warehouse.Agent.Name
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
            },
            Agent = new AgentResponse
            {
                Id = foundWarehouse.Agent.Id,
                Name = foundWarehouse.Agent.Name
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
