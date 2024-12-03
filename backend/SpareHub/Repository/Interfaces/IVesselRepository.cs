using Domain.Models;
using Shared.DTOs.Vessel;

namespace Repository.Interfaces;

public interface IVesselRepository
{ 
    Task<List<VesselResponse>> GetVesselsBySearchQueryAsync(string? searchQuery = "");
    
    Task<List<Vessel>> GetVesselsAsync();
    
    Task<Vessel> GetVesselByIdAsync(string vesselId);
    
    Task<Vessel> CreateVesselAsync(Vessel vessel);
    
    Task UpdateVesselAsync(string vesselId, Vessel vessel);
    
    Task DeleteVesselAsync(string vesselId);
}