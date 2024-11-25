using Microsoft.AspNetCore.Mvc;
using Service;
using Shared.DTOs.Vessel;

namespace Server;

[ApiController]
[Route("api/vessel")]
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

    [HttpPost]
    public async Task<IActionResult> CreateVessel(VesselRequest vesselRequest)
    {
        try
        {
            var vessel = await vesselService.CreateVessel(vesselRequest);

            return Ok(vessel);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{vesselId}")]
    public async Task<IActionResult> GetVesselById(string vesselId)
    {
        try
        {
            var vessel = await vesselService.GetVesselById(vesselId);

            return Ok(vessel);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut("{vesselId}")]
    public async Task<IActionResult> UpdateVessel(string vesselId, VesselRequest vesselRequest)
    {
        try
        {
            var vessel = await vesselService.UpdateVessel(vesselId, vesselRequest);

            return Ok(vessel);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpDelete("{vesselId}")]
    public async Task<IActionResult> DeleteVessel(string vesselId)
    {
        try
        {
            await vesselService.DeleteVessel(vesselId);

            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
