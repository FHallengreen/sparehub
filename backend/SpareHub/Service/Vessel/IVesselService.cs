using Shared;

namespace Service;

public interface IVesselService
{
    public Task<List<VesselResponse>> GetAllVessels();
}
