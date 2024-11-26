using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Interfaces;
using Shared.DTOs.Vessel;
using Shared.Exceptions;

namespace Server.VesselController;

[ApiController]
[Route("api/vessel")]
    public class VesselController(IDatabaseFactory databaseFactory) : ControllerBase
    {
        private readonly IVesselService _vesselService = databaseFactory.GetService<IVesselService>();
        
    

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(VesselResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> CreateVessel([FromBody] VesselRequest vesselRequest)
    {
        var vessel = await _vesselService.CreateVessel(vesselRequest);
        return CreatedAtAction(nameof(GetVesselById), new { vesselId = vessel.Id }, vessel);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetVessels()
    {
        var vessels = await _vesselService.GetVessels();

        return Ok(vessels);
        
    }
    
    [HttpGet("{vesselId}")]
    public async Task<IActionResult> GetVesselById(string vesselId)
    {
        var vessel = await _vesselService.GetVesselById(vesselId);

        return Ok(vessel);
       
    }

    [HttpPut("{vesselId}")]
    public async Task<IActionResult> UpdateVessel(string vesselId, [FromBody]VesselRequest vesselRequest)
    {
        var vessel = await _vesselService.UpdateVessel(vesselId, vesselRequest);

        return Ok(vessel);
    }
    
    [HttpDelete("{vesselId}")]
    public async Task<IActionResult> DeleteVessel(string vesselId)
    {
        await _vesselService.DeleteVessel(vesselId);

        return Ok();
    }
}
