using Microsoft.AspNetCore.Mvc;
using Service;
using Shared;

namespace Server;

[ApiController]
[Route("api/vessels")]
public class VesselController(IVesselService vesselService) : ControllerBase
{
    
    [HttpGet]
    public async Task<IActionResult> GetVessels(string? searchQuery = null)
    {
        var vessels = await vesselService.GetVesselsBySearchQuery(searchQuery);
        return Ok(vessels);
    }
}
