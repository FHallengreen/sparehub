using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Interfaces;
using Shared.DTOs.Port;

namespace Server.PortController;

[ApiController] 
[Route("api/vessel-port")]
    public class PortController(IDatabaseFactory databaseFactory) : ControllerBase
    {
        private readonly IPortService _portService = databaseFactory.GetService<IPortService>();
        
        
        [HttpGet]
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
        
        [HttpGet]
        public async Task<IActionResult> GetPortById(string vesselId)
        {
            var port = await _portService.GetPortById(vesselId);
            return Ok(port);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreatePort(PortRequest portRequest)
        {
            var port = await _portService.CreatePort(portRequest);
            return Ok(port);
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdatePort(string portId, PortRequest portRequest)
        {
            await _portService.UpdatePort(portId, portRequest);
            return Ok("Port updated successfully");
        }
        
        [HttpDelete]
        public async Task<IActionResult> DeletePort(string vesselId)
        {
            await _portService.DeletePort(vesselId);
            return Ok("Port deleted successfully");
        }
    }
