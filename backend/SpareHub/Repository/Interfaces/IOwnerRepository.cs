using Domain.Models;
using Shared.DTOs.Owner;

namespace Repository.Interfaces;

public interface IOwnerRepository
{
    Task<List<OwnerResponse>> GetOwnersBySearchQuery(string? searchQuery = "");
    Task<List<Owner>> GetOwnersAsync();
    
    Task<Owner> GetOwnerByIdAsync(string OwnerId);
    
    Task<Owner> CreateOwnerAsync(Owner owner);
    
    Task UpdateOwnerAsync(string ownerId, Owner owner);
    
    Task DeleteOwnerAsync(string ownerId);
}