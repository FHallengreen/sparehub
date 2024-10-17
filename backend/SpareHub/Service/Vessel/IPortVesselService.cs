using Shared;

namespace Service;

public interface IPortVesselService
{
    public Task<VesselAtPortDto> GetVesselsAtPortAsync(string portName);
}
