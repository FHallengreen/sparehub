using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Shared.DTOs.Vessel;
using Shared.Exceptions;

namespace Server.VesselController;

[ApiController]
[Route("api/vessel")]
[Authorize]
public class VesselController(IVesselService vesselService) : ControllerBase
{
    [HttpGet("query")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> GetVesselsBySearchQuery(string? searchQuery = null)
    {
        var vessels = await vesselService.GetVesselsBySearchQuery(searchQuery);

        return Ok(vessels);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(VesselResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> CreateVessel([FromBody] VesselRequest vesselRequest)
    {
        var vessel = await vesselService.CreateVessel(vesselRequest);
        return CreatedAtAction(nameof(GetVesselById), new { vesselId = vessel.Id }, vessel);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> GetVessels()
    {
        var vessels = await vesselService.GetVessels();

        return Ok(vessels);
    }

    [HttpGet("{vesselId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> GetVesselById(string vesselId)
    {
        var vessel = await vesselService.GetVesselById(vesselId);

        return Ok(vessel);
    }

    [HttpPut("{vesselId}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(VesselResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> UpdateVessel(string vesselId, [FromBody] VesselRequest vesselRequest)
    {
        Console.WriteLine("Updating vessel with id: " + vesselId);
        Console.WriteLine("Request: " + vesselRequest);
        var vessel = await vesselService.UpdateVessel(vesselId, vesselRequest);

        return Ok(vessel);
    }

    [HttpDelete("{vesselId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> DeleteVessel(string vesselId)
    {
        await vesselService.DeleteVessel(vesselId);

        return Ok();
    }
}