using Domain.Models;
using Shared.DTOs.Vessel;
using Shared.DTOs.VesselAtPort;

namespace Repository.Interfaces;

public interface IVesselAtPortRepository
{
    //Task<List<VesselAtPortResponse>> GetVesselsAtPortBySearchQuery(string? searchQuery = "");
    Task<List<VesselAtPort>> GetVesselAtPortAsync();
    
    Task<VesselAtPort> GetVesselByIdAtPortAsync(string vesselId);
    
    Task<VesselAtPort> AddVesselToPortAsync(VesselAtPort vesselAtPort);
    
    Task<VesselAtPortResponse> ChangePortForVesselAsync(VesselAtPort vesselAtPort, string newPortId);
    
    Task RemoveVesselFromPortAsync(string vesselId);
    
}