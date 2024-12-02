using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Shared.DTOs.Agent;
using Shared.DTOs.Order;

namespace Server.AgentController;

[ApiController]
[Route("api/agent")]
public class AgentController(IAgentService agentService) : ControllerBase
{
    
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AgentResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetAgents(string? searchQuery = null)
    {
        if (!string.IsNullOrEmpty(searchQuery))
        {
            var agents = await agentService.GetAgentsBySearchQuery(searchQuery);
            return Ok(agents);
        }
        var allAgents = await agentService.GetAgents();
        return Ok(allAgents);
    }

    [HttpGet("{agentId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgentResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetAgentById(string agentId)
    {
        var agent = await agentService.GetAgentById(agentId);
        return Ok(agent);
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AgentResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> CreateAgent([FromBody] AgentRequest agentRequest) {
        
        var createdAgent = await agentService.CreateAgent(agentRequest);
        return CreatedAtAction(nameof(GetAgentById), new { agentId = createdAgent.Id }, createdAgent);
    }
    
    [HttpPut("{agentId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgentResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> UpdateAgent(string agentId, [FromBody] AgentRequest agentRequest)
    {
        var updatedAgent = await agentService.UpdateAgent(agentId, agentRequest);
        return Ok(updatedAgent);
    }
    
    [HttpDelete("{agentId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> DeleteAgent(string agentId)
    {
        await agentService.DeleteAgent(agentId);
        return Ok("Agent deleted successfully");
    }
}
