using Shared.DTOs.Owner;

namespace Service.Interfaces;

public interface IOwnerService
{
    Task<List<OwnerResponse>> GetOwnersBySearchQuery(string? searchQuery = "");
    Task<List<OwnerResponse>> GetOwners();
    
    Task<OwnerResponse> GetOwnerById(string ownerId);
    
    Task<OwnerResponse> CreateOwner(OwnerRequest ownerRequest);
    
    Task<OwnerResponse> UpdateOwner(string ownerId, OwnerRequest ownerRequest);
    
    Task DeleteOwner(string ownerId);
}