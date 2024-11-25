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
        var agentEntity = await dbContext.Agents
            .FirstOrDefaultAsync(a => a.Id == Int32.Parse(id));
        return mapper.Map<Agent>(agentEntity);
    }
}