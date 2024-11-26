using Domain.Models;
using Shared.DTOs.Vessel;

namespace Service.Interfaces;

public interface IVesselService
{
    //Task<List<VesselResponse>> GetVesselsBySearchQuery(string? searchQuery = "");
    Task<List<VesselResponse>> GetVessels();
    Task<VesselResponse> GetVesselById(string vesselId);
    Task<VesselResponse> CreateVessel(VesselRequest vesselRequest);
    Task<VesselResponse> UpdateVessel(string vesselId, VesselRequest vesselRequest);
    Task DeleteVessel(string vesselId);
}
