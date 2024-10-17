using Microsoft.AspNetCore.Mvc;
using Service;
using Shared;

namespace Server;

[ApiController]
[Route("api/vessel")]
public class VesselController(IVesselService vesselService) : ControllerBase
{
    
    [HttpGet]
    public async Task<IActionResult> GetVessels()
    {
        var vessels = await vesselService.GetAllVessels();
        return Ok(vessels);
    }
}
