using Shared.DTOs.Order;

namespace Service.Interfaces;

public interface IAgentService
{
    Task<List<AgentResponse>> GetAgentsBySearchQuery(string? searchQuery);
}
