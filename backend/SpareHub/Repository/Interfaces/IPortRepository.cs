using Domain.Models;
using Shared.DTOs.Port;
using Shared.DTOs.Vessel;


namespace Repository.Interfaces;

public interface IPortRepository
{
    Task<List<PortResponse>> GetPortsBySearchQueryAsync(string? searchQuery = "");
    Task<Port> CreatePortAsync(Port port);
    
    Task<List<Port>> GetPortsAsync();
    
    Task<Port> GetPortByIdAsync(string portId);
    
    Task UpdatePortAsync(string portId, Port port);
    
    Task DeletePortAsync(string portId);
}