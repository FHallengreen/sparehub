using Microsoft.EntityFrameworkCore;
using Persistence;
using Shared;

namespace Service;

public class VesselService(SpareHubDbContext dbContext) : IVesselService
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
}
