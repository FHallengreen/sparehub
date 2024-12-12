using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Shared.DTOs.Port;
using Shared.Exceptions;

namespace Server.PortController;

[ApiController] 
[Route("api/port")]
[Authorize]
    public class PortController(IPortService portService) : ControllerBase
    {
        
        [HttpGet("query")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetPortsBySearchQuery(string? searchQuery = null)
        {
            var vessels = await portService.GetPortsBySearchQuery(searchQuery);

            return Ok(vessels);
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetPorts()
        {
            var ports = await portService.GetPorts();
            return Ok(ports);
        }
        
        [HttpGet ("{portId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetPortById(string portId)
        {
            var port = await portService.GetPortById(portId);
            return Ok(port);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PortResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> CreatePort(PortRequest portRequest)
        {
            var port = await portService.CreatePort(portRequest);
            return CreatedAtAction(nameof(GetPortById), new { portId = port.Id }, port);
        }
        
        [HttpPut ("{portId}")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PortResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> UpdatePort(string portId, [FromBody]PortRequest portRequest)
        {
            var port = await portService.UpdatePort(portId, portRequest);
            return Ok(port);
        }
        
        [HttpDelete ("{portId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> DeletePort(string portId)
        {
            await portService.DeletePort(portId);
            
            return Ok("Port deleted successfully");
            
        }
    }
