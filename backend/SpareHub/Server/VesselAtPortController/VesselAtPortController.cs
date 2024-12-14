using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;
using Service.Interfaces;
using Shared.DTOs.VesselAtPort;
using Shared.Exceptions;

namespace Server.VesselAtPortController;

[ApiController]
[Route("api/vessel-at-port")]
[Authorize]
public class VesselAtPortController(IVesselAtPortService vesselAtPortService) : ControllerBase
{
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> GetVesselAtPorts(IVesselRepository vesselRepository)
    {
        var vesselAtPorts = await vesselAtPortService.GetVesselAtPorts(vesselRepository);
        
        return Ok(vesselAtPorts);
    }
    
    [HttpGet("{vesselId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> GetVesselByIdAtPort(string vesselId, IVesselRepository vesselRepository)
    {
        var vesselAtPort = await vesselAtPortService.GetVesselByIdAtPort(vesselId, vesselRepository);
        
        return Ok(vesselAtPort);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(VesselAtPortResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> AddVesselToPort([FromBody] VesselAtPortRequest vesselAtPortRequest, 
        IVesselRepository vesselRepository)
    {
        var vesselAtPort = await vesselAtPortService.AddVesselToPort(vesselAtPortRequest, vesselRepository);
        Console.WriteLine("Arrival: " + vesselAtPort.ArrivalDate);
        Console.WriteLine("Departure: " + vesselAtPort.DepartureDate);
        
        return CreatedAtAction(nameof(GetVesselByIdAtPort), 
            new { vesselId = vesselAtPort.Vessels[0].Id }, vesselAtPort);
    }
    
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(VesselAtPortResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> ChangePortForVessel([FromBody] VesselAtPortRequest vesselAtPortRequest, 
        IVesselRepository vesselRepository)
    {
        var vesselAtPort = await vesselAtPortService.ChangePortForVessel(vesselAtPortRequest, vesselRepository);
        
        return Ok(vesselAtPort);
    }
    
    [HttpDelete("{vesselId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> RemoveVesselFromPort(string vesselId)
    {
        await vesselAtPortService.RemoveVesselFromPort(vesselId);
        
        return Ok($"Vessel with id '{vesselId}' removed from port");
    }
}