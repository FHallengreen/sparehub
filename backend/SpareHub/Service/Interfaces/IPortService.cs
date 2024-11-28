using Shared.DTOs.Port;


namespace Service.Interfaces;

public interface IPortService
{
    
    Task<List<PortResponse>> GetPorts();

    Task<PortResponse> GetPortById(string vesselId);
    
    Task<PortResponse> CreatePort(PortRequest portRequest);
    
    Task<PortResponse> UpdatePort(string portId, PortRequest portRequest);
    
    Task DeletePort(string vesselId);

}
