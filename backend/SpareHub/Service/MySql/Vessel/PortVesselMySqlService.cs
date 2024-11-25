using Neo4j.Driver;
using Shared.DTOs.Vessel;


namespace Service.MySql.Vessel
{
    public class PortVesselService(IAsyncSession session, IVesselService vesselService) : IPortVesselService
    {
        public Task<VesselAtPortRequest> GetVesselsAtPortAsync(string portName)
        {
            throw new NotImplementedException();
        }

        public Task<VesselResponse> GetVesselByIdAsync(string vesselId)
        {
            throw new NotImplementedException();
        }

        public Task<VesselResponse> CreateVesselAtPortAsync(VesselAtPortRequest vesselAtPortRequest)
        {
            throw new NotImplementedException();
        }

        public Task<VesselResponse> UpdateVesselAtPortAsync(string vesselId, VesselAtPortRequest vesselAtPortRequest)
        {
            throw new NotImplementedException();
        }

        public Task DeleteVesselAtPortAsync(string vesselId)
        {
            throw new NotImplementedException();
        }
    }
}
