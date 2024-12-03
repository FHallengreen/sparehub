using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Interfaces;
using Shared;

namespace Server
{
    [ApiController]
    [Route("api/vessel-port")]
    [Authorize]
    public class PortVesselController(IPortVesselService portVesselService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetVesselsAtPort(string portName)
        {
            var vessels = await portVesselService.GetVesselsAtPortAsync(portName);
            return Ok(vessels);
        }
    }
}
