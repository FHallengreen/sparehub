using Microsoft.EntityFrameworkCore;
using Persistence;
using Shared;

namespace Service;

public class VesselService(SpareHubDbContext dbContext) : IVesselService
{
    
    public async Task<List<VesselResponse>> GetAllVessels()
    {
        return await dbContext.Vessels
            .Select(v => new VesselResponse
            {
                Id = v.Id,
                Name = v.Name,
                ImoNumber = v.ImoNumber,
                Flag = v.Flag
            })
            .ToListAsync();
    }



}
