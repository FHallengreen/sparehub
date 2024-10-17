using Microsoft.AspNetCore.Mvc;
using Service;
using Shared;

namespace Server
{
    [ApiController]
    [Route("api/vessel-port")]
    public class PortVesselController(IPortVesselService portVesselService) : ControllerBase
    {
        [HttpGet("vessels-at-port")]
        public async Task<IActionResult> GetVesselsAtPort(string portName)
        {
            var vessels = await portVesselService.GetVesselsAtPortAsync(portName);
            return Ok(vessels);
        }
    }
}
