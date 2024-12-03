using Repository.MySql;
using Service.MySql.Vessel;
using Shared.DTOs.VesselAtPort;

namespace Service.Interfaces;

public interface IVesselAtPortService
{
    Task<List<VesselAtPortResponse>> GetVesselAtPorts(VesselMySqlRepository vesselMySqlRepository);
    
    Task<VesselAtPortResponse> GetVesselByIdAtPort(string vesselId, VesselMySqlRepository vesselMySqlRepository);
    
    Task<VesselAtPortResponse> AddVesselToPort(VesselAtPortRequest vesselAtPortRequest, 
        VesselMySqlRepository vesselMySqlRepository);
    
    Task<VesselAtPortResponse> ChangePortForVesselAsync(VesselAtPortRequest vesselAtPortRequest, 
        VesselMySqlRepository vesselMySqlRepository);
    
    Task RemoveVesselFromPort(string vesselId);
    
}