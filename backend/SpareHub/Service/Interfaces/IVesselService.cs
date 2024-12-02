using Shared;

namespace Service.Interfaces;

public interface IVesselService
{
    public Task<List<VesselResponse>> GetVesselsBySearchQuery(string? searchQuery);
}
