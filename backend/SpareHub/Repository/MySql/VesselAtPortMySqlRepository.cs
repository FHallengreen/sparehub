using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;
using Persistence.MySql;
using Persistence.MySql.SparehubDbContext;
using Repository.Interfaces;
using Shared.DTOs.VesselAtPort;
using Shared.Exceptions;

namespace Repository.MySql;

public class VesselAtPortMySqlRepository(SpareHubDbContext dbContext, IMapper mapper) : IVesselAtPortRepository
{
    
    public async Task<List<VesselAtPort>> GetVesselAtPortAsync()
    {
        var vesselAtPortEntities = await dbContext.VesselAtPort
            .Include(v => v.VesselEntity)
            .OrderBy(v => v.PortId)
            .ToListAsync();

        var mappedVesselsAtPort = mapper.Map<List<VesselAtPort>>(vesselAtPortEntities);
        return mappedVesselsAtPort;
    }

    public async Task<VesselAtPort> GetVesselByIdAtPortAsync(string vesselId)
    {
        var vesselAtPortEntity = await dbContext.VesselAtPort
            .Include(v => v.VesselEntity)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.VesselId.ToString() == vesselId);

        var vesselAtPort = mapper.Map<VesselAtPort>(vesselAtPortEntity);
        return vesselAtPort;
    }

    public async Task<VesselAtPort> AddVesselToPortAsync(VesselAtPort vesselAtPort)
    {
        var vesselAtPortEntity = mapper.Map<VesselAtPortEntity>(vesselAtPort);
        await dbContext.VesselAtPort.AddAsync(vesselAtPortEntity);
        await dbContext.SaveChangesAsync();

        return mapper.Map<VesselAtPort>(vesselAtPortEntity);
    }

    // This method is used to change the port of a vessel
    // You input the vesselAtPort object and the new portId
    public async Task<VesselAtPortResponse> ChangePortForVesselAsync(VesselAtPort vesselAtPort, string newPortId)
    {
        await RemoveVesselFromPortAsync(vesselAtPort.VesselId);
        vesselAtPort.PortId = newPortId;
        var updatedVesselAtPort = await AddVesselToPortAsync(vesselAtPort);
        return mapper.Map<VesselAtPortResponse>(updatedVesselAtPort);
    }

    public async Task RemoveVesselFromPortAsync(string vesselId)
    {
        var vesselAtPortEntity =
            await dbContext.VesselAtPort.FirstOrDefaultAsync(p => p.VesselId.ToString() == vesselId);

        if (vesselAtPortEntity == null)
            throw new NotFoundException($"Vessel with id '{vesselId}' not found at port");

        dbContext.VesselAtPort.Remove(vesselAtPortEntity);
        await dbContext.SaveChangesAsync();
    }
}