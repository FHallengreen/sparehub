using Shared.DTOs.Port;
using Shared.DTOs.Vessel;


namespace Service.Interfaces;

public interface IPortService
{
    
    public Task<List<PortResponse>> GetPorts();

    public Task<PortRequest> GetPortById(string vesselId);
    
    public Task<PortResponse> CreatePort(PortRequest portRequest);
    
    public Task UpdatePort(string portId, PortRequest portRequest);
    
    public Task DeletePort(string vesselId);

}
