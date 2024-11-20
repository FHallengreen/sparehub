using Shared;

namespace Service;

public interface IVesselService
{
    public Task<List<VesselResponse>> GetVesselsBySearchQuery(string? searchQuery);
    Task<VesselResponse> GetVesselById(string vesselId);
    Task<VesselResponse> CreateVessel(CreateVesselDto createVesselDto);
    Task<VesselResponse> UpdateVessel(string vesselId, CreateVesselDto createVesselDto);
    Task DeleteVessel(string vesselId);
}
