using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Server;

[ApiController]
[Route("api/agents")]
public class AgentController(IAgentService agentService) : ControllerBase
{
    [HttpGet("search")]
    public async Task<IActionResult> GetAgents(string? searchQuery = null)
    {
        try
        {
            var agents = await agentService.GetAgentsBySearchQuery(searchQuery);
            if (!agents.Any())
            {
                return NotFound("No agents found matching the search criteria.");
            }

            return Ok(agents);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
