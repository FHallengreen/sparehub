using AutoMapper;
using Domain.MySql;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.MySql;
using Repository.Interfaces;

namespace Repository.MySql;

public class VesselMySqlRepository(SpareHubDbContext dbContext, IMapper mapper) : IVesselRepository
{

    public async Task<List<Vessel>> GetVesselsAsync()
    {
        var vesselEntitiesWithOwners = await dbContext.Vessels
            .Include(v => v.Owner)
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
        throw new NotImplementedException();
    }
    

    public async Task UpdateVesselAsync(string vesselId, Vessel vessel)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteVesselAsync(string vesselId)
    {
        throw new NotImplementedException();
    }
}