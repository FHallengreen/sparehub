using Shared;

namespace Service;

public interface IVesselService
{
    public Task<List<VesselResponse>> GetVesselsBySearchQuery(string? searchQuery);
}
