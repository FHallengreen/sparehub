using Microsoft.EntityFrameworkCore;
using Persistence;
using Shared;
using Shared.DTOs.Vessel;

namespace Service.MySql.Vessel;

public class VesselMySqlService(SpareHubDbContext dbContext) : IVesselService
{
    public async Task<List<VesselResponse>> GetVesselsBySearchQuery(string? searchQuery = "")
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

    public Task<VesselResponse> GetVesselById(string vesselId)
    {
        throw new NotImplementedException();
    }

    public Task<VesselResponse> CreateVessel(VesselRequest vesselRequest)
    {
        throw new NotImplementedException();
    }

    public Task<VesselResponse> UpdateVessel(string vesselId, VesselRequest vesselRequest)
    {
        throw new NotImplementedException();
    }

    public Task DeleteVessel(string vesselId)
    {
        throw new NotImplementedException();
    }
}
