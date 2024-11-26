using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.MySql;
using Repository.Interfaces;
using Shared.DTOs.Vessel;

namespace Repository.MySql;

public class PortMySqlRepository(SpareHubDbContext dbContext, IMapper mapper) : IPortRepository
{

    public async Task<VesselAtPortRequest> GetVesselsAtPortAsync(string portId)
    {
        //step 1: Find ID of port
        

        //step 2: Find all vessels at port
        

        //step 3: Return list of vessels at port
        throw new NotImplementedException();
    }

    public async Task<Port> CreatePortAsync(Port port)
    {
        var portEntity = mapper.Map<PortEntity>(port);
        dbContext.Ports.Add(portEntity);
        await dbContext.SaveChangesAsync();
        port.Id = portEntity.Id.ToString();
        return port;
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
        portEntity.Id = int.Parse(portId);
        dbContext.Ports.Update(portEntity);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeletePortAsync(string portId)
    {
        dbContext.Ports.Remove(new PortEntity { Id = int.Parse(portId) });
        await dbContext.SaveChangesAsync();
    }

}