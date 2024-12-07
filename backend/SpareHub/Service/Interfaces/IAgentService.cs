using Shared.DTOs.Agent;
using Shared.DTOs.Order;

namespace Service.Interfaces;

public interface IAgentService
{
    Task<List<AgentResponse>> GetAgents();
    Task<List<AgentResponse>> GetAgentsBySearchQuery(string? searchQuery);
    
    Task<AgentResponse> GetAgentById(string agentId);
    Task<AgentResponse> CreateAgent(AgentRequest request);
    
    Task<AgentResponse> UpdateAgent(string agentId, AgentRequest request);
    
    Task DeleteAgent(string agentId);
    
}
