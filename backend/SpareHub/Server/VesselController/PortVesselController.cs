using Microsoft.AspNetCore.Mvc;
using Service;
using Shared;

namespace Server
{
    [ApiController]
    [Route("api/vessel-port")]
    public class PortVesselController(IPortVesselService portVesselService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetVesselsAtPort(string portName)
        {
            var vessels = await portVesselService.GetVesselsAtPortAsync(portName);
            return Ok(vessels);
        }

        [HttpGet("{vesselId}")]
        public async Task<IActionResult> GetVesselById(string vesselId)
        {
            var vessel = await portVesselService.GetVesselByIdAsync(vesselId);
            return Ok(vessel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVesselAtPort(VesselAtPortDto vesselAtPortDto)
        {
            var vessel = await portVesselService.CreateVesselAtPortAsync(vesselAtPortDto);
            return Ok(vessel);
        }

        [HttpPut("{vesselId}")]
        public async Task<IActionResult> UpdateVesselAtPort(string vesselId, VesselAtPortDto vesselAtPortDto)
        {
            var vessel = await portVesselService.UpdateVesselAtPortAsync(vesselId, vesselAtPortDto);
            return Ok(vessel);
        }

        [HttpDelete("{vesselId}")]
        public async Task<IActionResult> DeleteVesselAtPort(string vesselId)
        {
            await portVesselService.DeleteVesselAtPortAsync(vesselId);
            return Ok();
        }
    }


}
