using Microsoft.AspNetCore.Mvc;
using Repository.MySql;
using Service;
using Service.Interfaces;
using Shared.DTOs.VesselAtPort;
using Shared.Exceptions;

namespace Server.VesselAtPortController;

[ApiController]
[Route("api/vessel-at-port")]
public class VesselAtPortController(IDatabaseFactory databaseFactory) : ControllerBase
{
    private readonly IVesselAtPortService _vesselAtPortService = databaseFactory.GetRepository<IVesselAtPortService>();
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> GetVesselAtPorts(VesselMySqlRepository vesselMySqlRepository)
    {
        var vesselAtPorts = await _vesselAtPortService.GetVesselAtPorts(vesselMySqlRepository);
        
        return Ok(vesselAtPorts);
    }
    
    [HttpGet("{vesselId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> GetVesselByIdAtPort(string vesselId, VesselMySqlRepository vesselMySqlRepository)
    {
        var vesselAtPort = await _vesselAtPortService.GetVesselByIdAtPort(vesselId, vesselMySqlRepository);
        
        return Ok(vesselAtPort);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(VesselAtPortResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> AddVesselToPort([FromBody] VesselAtPortRequest vesselAtPortRequest, 
        VesselMySqlRepository vesselMySqlRepository)
    {
        var vesselAtPort = await _vesselAtPortService.AddVesselToPort(vesselAtPortRequest, vesselMySqlRepository);
        
        return CreatedAtAction(nameof(GetVesselByIdAtPort), 
            new { vesselId = vesselAtPort.Vessels[0].Id }, vesselAtPort);
    }
    
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(VesselAtPortResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> ChangePortForVessel([FromBody] VesselAtPortRequest vesselAtPortRequest, 
        VesselMySqlRepository vesselMySqlRepository)
    {
        var vesselAtPort = await _vesselAtPortService.ChangePortForVesselAsync(vesselAtPortRequest, vesselMySqlRepository);
        
        return Ok(vesselAtPort);
    }
    
    [HttpDelete("{vesselId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> RemoveVesselFromPort(string vesselId)
    {
        await _vesselAtPortService.RemoveVesselFromPort(vesselId);
        
        return Ok($"Vessel with id '{vesselId}' removed from port");
    }
}