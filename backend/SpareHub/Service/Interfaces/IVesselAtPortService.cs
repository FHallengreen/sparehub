using Repository.Interfaces;
using Shared.DTOs.VesselAtPort;

namespace Service.Interfaces;

public interface IVesselAtPortService
{
    Task<List<VesselAtPortResponse>> GetVesselAtPorts(IVesselRepository vesselRepository);
    
    Task<VesselAtPortResponse> GetVesselByIdAtPort(string vesselId, IVesselRepository vesselRepository);
    
    Task<VesselAtPortResponse> AddVesselToPort(VesselAtPortRequest vesselAtPortRequest, 
        IVesselRepository vesselRepository);
    
    Task<VesselAtPortResponse> ChangePortForVessel(VesselAtPortRequest vesselAtPortRequest,
        IVesselRepository vesselRepository);
    
    Task RemoveVesselFromPort(string vesselId);
    
}