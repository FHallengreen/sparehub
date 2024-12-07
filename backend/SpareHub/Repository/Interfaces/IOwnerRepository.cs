using Domain.Models;

namespace Repository.Interfaces;

public interface IOwnerRepository
{
    Task<List<Owner>> GetOwnersAsync();
    
    Task<Owner> GetOwnerByIdAsync(string OwnerId);
    
    Task<Owner> CreateOwnerAsync(Owner owner);
    
    Task UpdateOwnerAsync(string ownerId, Owner owner);
    
    Task DeleteOwnerAsync(string ownerId);
}