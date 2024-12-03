using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Interfaces;
using Shared;

namespace Server;

[ApiController]
[Route("api/vessel")]
[Authorize]
public class VesselController(IVesselService vesselService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetVessels(string? searchQuery = null)
    {
        try
        {
            var vessels = await vesselService.GetVesselsBySearchQuery(searchQuery);

            return Ok(vessels);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
