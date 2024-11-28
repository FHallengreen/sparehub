using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.MySql;
using Repository.Interfaces;
using Shared.Exceptions;

namespace Repository.MySql;

public class PortMySqlRepository(SpareHubDbContext dbContext, IMapper mapper) : IPortRepository
{

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
        
        dbContext.Ports.Remove(new PortEntity {Id = int.Parse(portId)});
        await dbContext.SaveChangesAsync();
    }

}