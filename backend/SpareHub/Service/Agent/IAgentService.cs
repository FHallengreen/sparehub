using Shared;

namespace Service.Agent;

public interface IAgentService
{
    Task<List<AgentResponse>> GetAgentsBySearchQuery(string? searchQuery);
}
