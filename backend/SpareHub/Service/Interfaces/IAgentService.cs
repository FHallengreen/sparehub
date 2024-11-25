using Shared;
using Shared.DTOs.Order;

namespace Service.Agent;

public interface IAgentService
{
    Task<List<AgentResponse>> GetAgentsBySearchQuery(string? searchQuery);
}
