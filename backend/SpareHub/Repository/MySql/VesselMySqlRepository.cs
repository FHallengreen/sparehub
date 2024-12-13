using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.MySql;
using Persistence.MySql.SparehubDbContext;
using Repository.Interfaces;
using Shared.DTOs.Owner;
using Shared.DTOs.Vessel;
using Shared.Exceptions;

namespace Repository.MySql;

public class VesselMySqlRepository(SpareHubDbContext dbContext, IMapper mapper) : IVesselRepository
{
    
    public async Task<List<VesselResponse>> GetVesselsBySearchQueryAsync(string? searchQuery = "")
    {
        return await dbContext.Vessels
            .Where(v => string.IsNullOrEmpty(searchQuery) || v.Name.StartsWith(searchQuery))
            .Include(v => v.Owner)
            .Select(v => new VesselResponse
            {
                Id = v.Id.ToString(),
                Name = v.Name,
                ImoNumber = v.ImoNumber,
                Flag = v.Flag,
                Owner = new OwnerResponse
                {
                    Id = v.Owner.Id.ToString(),
                    Name = v.Owner.Name
                }
            })
            .ToListAsync();
    }


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
            .AsNoTracking()
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