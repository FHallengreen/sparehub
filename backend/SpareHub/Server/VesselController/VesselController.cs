using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Interfaces;
using Shared.DTOs.Vessel;

namespace Server.VesselController;

[ApiController]
[Route("api/vessel")]
    public class VesselController(IDatabaseFactory databaseFactory) : ControllerBase
    {
        private readonly IVesselService _vesselService = databaseFactory.GetService<IVesselService>();
        
    

    [HttpPost]
    public async Task<IActionResult> CreateVessel(VesselRequest vesselRequest)
    {
        try
        {
            var vessel = await _vesselService.CreateVessel(vesselRequest);

            return Ok(vessel);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetVessels()
    {
        try
        {
            var vessels = await _vesselService.GetVessels();

            return Ok(vessels);
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
            var vessel = await _vesselService.GetVesselById(vesselId);

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
            var vessel = await _vesselService.UpdateVessel(vesselId, vesselRequest);

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
            await _vesselService.DeleteVessel(vesselId);

            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
