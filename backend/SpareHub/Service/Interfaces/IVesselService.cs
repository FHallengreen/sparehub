using Domain.Models;
using Shared.DTOs.Vessel;

namespace Service.Interfaces;

public interface IVesselService
{
    Task<List<VesselResponse>> GetVessels();
    Task<VesselRequest> GetVesselById(string vesselId);
    Task<VesselResponse> CreateVessel(VesselRequest vesselRequest);
    Task<VesselResponse> UpdateVessel(string vesselId, VesselRequest vesselRequest);
    Task DeleteVessel(string vesselId);
}
