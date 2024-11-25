using Shared.DTOs.Vessel;

namespace Service;

public interface IVesselService
{
    public Task<List<VesselResponse>> GetVesselsBySearchQuery(string? searchQuery);
    Task<VesselResponse> GetVesselById(string vesselId);
    Task<VesselResponse> CreateVessel(VesselRequest vesselRequest);
    Task<VesselResponse> UpdateVessel(string vesselId, VesselRequest vesselRequest);
    Task DeleteVessel(string vesselId);
}
