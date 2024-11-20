using Microsoft.AspNetCore.Mvc;
using Service;
using Shared;

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
    public async Task<IActionResult> CreateVessel(CreateVesselDto createVesselDto)
    {
        try
        {
            var vessel = await vesselService.CreateVessel(createVesselDto);

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
    public async Task<IActionResult> UpdateVessel(string vesselId, CreateVesselDto createVesselDto)
    {
        try
        {
            var vessel = await vesselService.UpdateVessel(vesselId, createVesselDto);

            return Ok(vessel);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
