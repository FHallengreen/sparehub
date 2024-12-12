using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;
using Persistence.MySql;
using Persistence.MySql.SparehubDbContext;
using Repository.Interfaces;
using Shared.DTOs.Port;
using Shared.Exceptions;

namespace Repository.MySql;

public class PortMySqlRepository(SpareHubDbContext dbContext, IMapper mapper) : IPortRepository
{

    public async Task<List<PortResponse>> GetPortsBySearchQueryAsync(string? searchQuery = "")
    {
        return await dbContext.Ports
        .Where(v => string.IsNullOrEmpty(searchQuery) || v.Name.StartsWith(searchQuery))
        .Select(v => new PortResponse
        {
            Id = v.Id.ToString(),
            Name = v.Name,
        })
        .ToListAsync();
    }
    public async Task<Port> CreatePortAsync(Port port)
    {
        var portEntity = mapper.Map<PortEntity>(port);
        await dbContext.Ports.AddAsync(portEntity);
        await dbContext.SaveChangesAsync();
        
        return mapper.Map<Port>(portEntity);
    }

    public async Task<List<Port>> GetPortsAsync()
    {
        var portEntities = await dbContext.Ports.ToListAsync();
        var ports = mapper.Map<List<Port>>(portEntities);
        return ports;
    }

    public async Task<Port> GetPortByIdAsync(string portId)
    {
        var portEntity = await dbContext.Ports
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == int.Parse(portId));
            
        var port = mapper.Map<Port>(portEntity);
        return port;
    }

    public async Task UpdatePortAsync(string portId, Port port)
    {
        
        var portEntity = mapper.Map<PortEntity>(port);
        dbContext.Ports.Update(portEntity);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeletePortAsync(string portId)
    {
        var portEntity = await dbContext.Ports.FirstOrDefaultAsync(p => p.Id == int.Parse(portId));
        
        if (portEntity == null)
            throw new NotFoundException($"Port with id '{portId}' not found");
        
        dbContext.Ports.Remove(portEntity);
        await dbContext.SaveChangesAsync();
    }

}