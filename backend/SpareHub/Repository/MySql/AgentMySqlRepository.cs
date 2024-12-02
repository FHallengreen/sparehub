using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.MySql;
using Persistence.MySql.SparehubDbContext;
using Repository.Interfaces;
using Shared.Exceptions;

namespace Repository.MySql;

public class AgentMySqlRepository(SpareHubDbContext dbContext, IMapper mapper) : IAgentRepository
{
    public async Task<List<Agent>> GetAgentsAsync()
    {
        var agents = await dbContext.Agents.ToListAsync();
        return mapper.Map<List<Agent>>(agents);
    }
    public async Task<List<Agent>> GetAgentsBySearchQueryAsync(string? searchQuery)
    {
        
        var agents = await dbContext.Agents
            .Where(a => searchQuery != null && a.Name.Contains(searchQuery))
            .ToListAsync();

        return mapper.Map<List<Agent>>(agents);
    }

    public async Task<Agent> GetAgentByIdAsync(string id)
    {
        if (!int.TryParse(id, out var parsedId))
            throw new ArgumentException("Invalid agent ID format.");

        var agentEntity = await dbContext.Agents
            .FirstOrDefaultAsync(a => a.Id == parsedId);

        if (agentEntity == null)
            throw new KeyNotFoundException($"Agent with ID {id} not found.");

        return mapper.Map<Agent>(agentEntity);
    }

    public async Task<Agent> CreateAgentAsync(Agent agent)
    {
        var agentEntity = mapper.Map<AgentEntity>(agent);
        await dbContext.Agents.AddAsync(agentEntity);
        await dbContext.SaveChangesAsync();
        agent.Id = agentEntity.Id.ToString();
        
        return agent;
    }

    public async Task<Agent> UpdateAgentAsync(Agent agent)
    {
        var agentEntity = mapper.Map<AgentEntity>(agent);
        dbContext.Agents.Update(agentEntity);
        await dbContext.SaveChangesAsync();
        return mapper.Map<Agent>(agentEntity);
    }

    public async Task DeleteAgentAsync(string agentId)
    {
        if (!int.TryParse(agentId, out var id))
            return;

        var agentEntity = await dbContext.Agents
            .FirstOrDefaultAsync(d => d.Id == id);

        if (agentEntity == null)
            throw new NotFoundException("Agent not found");

        dbContext.Agents.Remove(agentEntity);
        await dbContext.SaveChangesAsync();
    }
}