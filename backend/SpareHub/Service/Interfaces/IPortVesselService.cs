using Shared.DTOs.Vessel;


namespace Service;

public interface IPortVesselService
{
    public Task<VesselAtPortDto> GetVesselsAtPortAsync(string portName);

    public Task<VesselResponse> GetVesselByIdAsync(string vesselId);
    public Task<VesselResponse> CreateVesselAtPortAsync(VesselAtPortDto VesselAtPortDto);
    public Task<VesselResponse> UpdateVesselAtPortAsync(string vesselId, VesselAtPortDto VesselAtPortDto);
    public Task DeleteVesselAtPortAsync(string vesselId);

}
