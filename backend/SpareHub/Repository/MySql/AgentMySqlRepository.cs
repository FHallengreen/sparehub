using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Models;
using Domain.MySql;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Repository.Interfaces;
using Shared;

namespace Repository.MySql;

public class AgentMySqlRepository(SpareHubDbContext dbContext, IMapper mapper) : IAgentRepository
{
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

}