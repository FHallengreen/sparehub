using Domain.Models;

namespace Repository.Interfaces;

public interface IVesselRepository
{ 
    Task<List<Vessel>> GetVesselsAsync();
    
    Task<Vessel> GetVesselByIdAsync(string vesselId);
    
    Task<Vessel> CreateVesselAsync(Vessel vessel);
    
    Task UpdateVesselAsync(string vesselId, Vessel vessel);
    
    Task DeleteVesselAsync(string vesselId);
}