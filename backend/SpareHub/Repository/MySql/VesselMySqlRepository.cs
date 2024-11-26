using AutoMapper;
using Domain.MySql;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.MySql;
using Repository.Interfaces;
using Shared.Exceptions;

namespace Repository.MySql;

public class VesselMySqlRepository(SpareHubDbContext dbContext, IMapper mapper) : IVesselRepository
{

    public async Task<List<Vessel>> GetVesselsAsync()
    {
        var vesselEntitiesWithOwners = await dbContext.Vessels
            .Include(v => v.Owner)
            .OrderBy(v => v.Id)
            .ToListAsync();
        
        var mappedVessels = mapper.Map<List<Vessel>>(vesselEntitiesWithOwners);

        return mappedVessels;
    }
    

    public async Task<Vessel> GetVesselByIdAsync(string vesselId)
    {
        var vesselEntityWithOwner = await dbContext.Vessels
            .Include(v => v.Owner)
            .FirstOrDefaultAsync(v => v.Id.ToString() == vesselId);
        
        var mappedVessel = mapper.Map<Vessel>(vesselEntityWithOwner);
        return mappedVessel;
    }

    public async Task<Vessel> CreateVesselAsync(Vessel vessel)
    {
        var vesselEntity = mapper.Map<VesselEntity>(vessel);
        await dbContext.Vessels.AddAsync(vesselEntity);
        await dbContext.SaveChangesAsync();
        
        return mapper.Map<Vessel>(vesselEntity);
    }
    

    public async Task UpdateVesselAsync(string vesselId, Vessel vessel)
    {
        var vesselEntity = mapper.Map<VesselEntity>(vessel);
        dbContext.Vessels.Update(vesselEntity);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteVesselAsync(string vesselId)
    {
        var vesselEntity = await dbContext.Vessels.FirstOrDefaultAsync(v => v.Id.ToString() == vesselId);
        
        if (vesselEntity == null)
            throw new NotFoundException($"Vessel with id '{vesselId}' not found");
        
        dbContext.Vessels.Remove(vesselEntity);
        await dbContext.SaveChangesAsync();
    }
}