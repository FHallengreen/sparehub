using Shared.DTOs.Vessel;


namespace Service;

public interface IPortVesselService
{
    public Task<VesselAtPortRequest> GetVesselsAtPortAsync(string portName);

    public Task<VesselResponse> GetVesselByIdAsync(string vesselId);
    public Task<VesselResponse> CreateVesselAtPortAsync(VesselAtPortRequest vesselAtPortRequest);
    public Task<VesselResponse> UpdateVesselAtPortAsync(string vesselId, VesselAtPortRequest vesselAtPortRequest);
    public Task DeleteVesselAtPortAsync(string vesselId);

}
