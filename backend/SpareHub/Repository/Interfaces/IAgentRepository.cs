using Domain.Models;

namespace Repository.Interfaces;

public interface IAgentRepository
{
    Task<List<Agent>> GetAgentsAsync();
    Task<List<Agent>> GetAgentsBySearchQueryAsync(string? searchQuery);
    Task<Agent> GetAgentByIdAsync(string id);
    
    Task<Agent> CreateAgentAsync(Agent agent);
    Task<Agent> UpdateAgentAsync(Agent agent);
    Task DeleteAgentAsync(string agentId);
}
