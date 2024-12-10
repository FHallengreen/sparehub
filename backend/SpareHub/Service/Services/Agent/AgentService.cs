using System.ComponentModel.DataAnnotations;
using Repository.Interfaces;
using Service.Interfaces;
using Shared.DTOs.Agent;
using Shared.DTOs.Order;
using Shared.Exceptions;

namespace Service.Services.Agent;

public class AgentService (IAgentRepository agentRepo) : IAgentService
{
    public async Task<List<AgentResponse>> GetAgents()
    {
        var agents = await agentRepo.GetAgentsAsync();
        
        return agents.Select(a => new AgentResponse
        {
            Id = a.Id,
            Name = a.Name,
        }).ToList();
    }
    public async Task<List<AgentResponse>> GetAgentsBySearchQuery(string? searchQuery)
    {
        var agents = await agentRepo.GetAgentsBySearchQueryAsync(searchQuery);
        
        return agents.Select(a => new AgentResponse
        {
            Id = a.Id,
            Name = a.Name,
        }).ToList();
    }

    public async Task<AgentResponse> GetAgentById(string agentId)
    {
        var foundAgent = await agentRepo.GetAgentByIdAsync(agentId);
        if (foundAgent == null)
        {
            throw new NotFoundException($"No agent found with id {agentId}");
        }
        
        return new AgentResponse
        {
            Id = foundAgent.Id,
            Name = foundAgent.Name,
        };
    }

    public async Task<AgentResponse> CreateAgent(AgentRequest request)
    {
        var agent = new Domain.Models.Agent
        {
            Name = request.Name
        };
        
        await agentRepo.CreateAgentAsync(agent);
        
        return new AgentResponse
        {
            Id = agent.Id,
            Name = agent.Name
        };
    }


    public async Task<AgentResponse> UpdateAgent(string agentId, AgentRequest request)
    {
        var foundAgent = await agentRepo.GetAgentByIdAsync(agentId);
        if (foundAgent == null)
        {
            throw new NotFoundException($"No agent found with id {agentId}");
        }
        
        foundAgent.Name = request.Name;
        
        await agentRepo.UpdateAgentAsync(foundAgent);
        
        return new AgentResponse
        {
            Id = foundAgent.Id,
            Name = foundAgent.Name
        };
    }

    public async Task DeleteAgent(string agentId)
    {
        if (string.IsNullOrWhiteSpace(agentId))
            throw new ValidationException("Warehouse ID cannot be null or empty.");

        await agentRepo.DeleteAgentAsync(agentId);
    }
}
