using AutoMapper;
using Domain.Models;
using Domain.MySql;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Repository.Interfaces;

namespace Repository.MySql;

public class VesselMySqlRepository(SpareHubDbContext dbContext, IMapper mapper) : IVesselRepository
{
    public async Task<List<Vessel>> GetVesselsAsync()
    {
        var vesselEntities = await dbContext.Vessels.ToListAsync();
        var vessels = mapper.Map<List<Vessel>>(vesselEntities);
        return vessels;
    }

    public async Task<Vessel> GetVesselByIdAsync(string vesselId)
    {
        var vesselEntity = await dbContext.Vessels
            .FirstOrDefaultAsync(v => v.Id == int.Parse(vesselId));
        
        var vessel = mapper.Map<Vessel>(vesselEntity);
        return vessel;
    }

    public async Task<Vessel> CreateVesselAsync(Vessel vessel)
    {
        var vesselEntity = mapper.Map<VesselEntity>(vessel);
        dbContext.Vessels.Add(vesselEntity);
        await dbContext.SaveChangesAsync();
        vessel.Id = vesselEntity.Id.ToString();
        return vessel;
    }

    public async Task UpdateVesselAsync(string vesselId, Vessel vessel)
    {
        var vesselEntity = mapper.Map<VesselEntity>(vessel);
        vesselEntity.Id = int.Parse(vesselId);
        dbContext.Vessels.Update(vesselEntity);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteVesselAsync(string vesselId)
    {
        var vesselEntity = await dbContext.Vessels
            .FirstOrDefaultAsync(v => v.Id == int.Parse(vesselId));

        if (vesselEntity != null) dbContext.Vessels.Remove(vesselEntity);
        await dbContext.SaveChangesAsync();
    }
}