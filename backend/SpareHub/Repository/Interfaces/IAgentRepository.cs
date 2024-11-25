using Domain.Models;
using Shared;

namespace Repository.Interfaces;

public interface IAgentRepository
{
    Task<Agent> GetAgentByIdAsync(string id);
}
