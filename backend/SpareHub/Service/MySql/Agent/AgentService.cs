using Microsoft.EntityFrameworkCore;
using Persistence;
using Shared;

namespace Service.Agent;

public class AgentService (SpareHubDbContext dbContext) : IAgentService
{
    public async Task<List<AgentResponse>> GetAgentsBySearchQuery(string? searchQuery)
    {
        return await dbContext.Agents
            .Where(a => string.IsNullOrEmpty(searchQuery) || a.Name.StartsWith(searchQuery))
            .Select(a => new AgentResponse
            {
                Id = a.Id.ToString(),
                Name = a.Name
            })
            .ToListAsync();
    }
}