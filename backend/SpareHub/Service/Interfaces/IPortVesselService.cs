using Shared;

namespace Service.Interfaces;

public interface IPortVesselService
{
    public Task<VesselAtPortDto> GetVesselsAtPortAsync(string portName);
}
