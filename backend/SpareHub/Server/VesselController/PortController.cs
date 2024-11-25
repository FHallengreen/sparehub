using Microsoft.AspNetCore.Mvc;
using Service;
using Shared.DTOs.Vessel;

namespace Server
{
    [ApiController]
    [Route("api/vessel-port")]
    public class PortController(IPortVesselService portVesselService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetVesselsAtPort(string portName)
        {
            var vessels = await portVesselService.GetVesselsAtPortAsync(portName);
            return Ok(vessels);
        }

        // create, delete, update port
        
        
    }


}
