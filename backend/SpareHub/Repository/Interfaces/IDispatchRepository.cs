using Domain.Models;

namespace Repository.Interfaces;

public interface IDispatchRepository
{
    Task<Dispatch> CreateDispatchAsync(Dispatch dispatch);
    Task<Dispatch?> GetDispatchByIdAsync(string dispatchId);
    Task<IEnumerable<Dispatch>> GetDispatchesAsync();
    Task<Dispatch> UpdateDispatchAsync(Dispatch dispatch);
    Task DeleteDispatchAsync(string dispatchId);
}