using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Interfaces;
using Shared.DTOs.Port;

namespace Server.PortController;

[ApiController] 
[Route("api/port")]
    public class PortController(IDatabaseFactory databaseFactory) : ControllerBase
    {
        private readonly IPortService _portService = databaseFactory.GetService<IPortService>();
        
        
        [HttpGet("{portId}/vessel")]
        public async Task<IActionResult> GetVesselsAtPort(string portId)
        {
            var vessels = await _portService.GetVesselsAtPort(portId);
            return Ok(vessels);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetPorts()
        {
            var ports = await _portService.GetPorts();
            return Ok(ports);
        }
        
        [HttpGet ("{portId}")]
        public async Task<IActionResult> GetPortById(string portId)
        {
            var port = await _portService.GetPortById(portId);
            return Ok(port);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreatePort(PortRequest portRequest)
        {
            var port = await _portService.CreatePort(portRequest);
            return Ok(port);
        }
        
        [HttpPut ("{portId}")]
        public async Task<IActionResult> UpdatePort(string portId, PortRequest portRequest)
        {
            await _portService.UpdatePort(portId, portRequest);
            return Ok("Port updated successfully");
        }
        
        [HttpDelete ("{portId}")]
        public async Task<IActionResult> DeletePort(string portId)
        {
            await _portService.DeletePort(portId);
            return Ok("Port deleted successfully");
        }
    }
