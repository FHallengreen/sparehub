using Repository.Interfaces;
using Service.Interfaces;
using Shared.DTOs.Owner;
using Shared.Exceptions;

namespace Service.MySql.Owner;

public class OwnerService(IOwnerRepository ownerRepository) : IOwnerService
{
    
    public async Task<List<OwnerResponse>> GetOwners()
    {
        var owners = await ownerRepository.GetOwnersAsync();

        if (owners == null || owners.Count == 0)
            throw new NotFoundException("No owners found");

        return owners.Select(o => new OwnerResponse
        {
            Id = o.Id,
            Name = o.Name
        }).ToList();
    }

    public async Task<OwnerResponse> GetOwnerById(string ownerId)
    {
        var owner = await ownerRepository.GetOwnerByIdAsync(ownerId);
        
        if (owner == null)
            throw new NotFoundException($"Owner with id '{ownerId}' not found");

        return new OwnerResponse()
        {
            Id = owner.Id,
            Name = owner.Name
        };
    }
    
    public async Task<OwnerResponse> CreateOwner(OwnerRequest ownerRequest)
    {
        var owner = new Domain.Models.Owner
        {
            Name = ownerRequest.Name
        };

        var createdOwner = await ownerRepository.CreateOwnerAsync(owner);

        return new OwnerResponse
        {
            Id = createdOwner.Id,
            Name = createdOwner.Name
        };
    }
    
    public async Task<OwnerResponse> UpdateOwner(string ownerId, OwnerRequest ownerRequest)
    {
        var owner = await ownerRepository.GetOwnerByIdAsync(ownerId);
        if (owner == null)
            throw new NotFoundException($"Owner with id '{ownerId}' not found");
        
        //Request the change of owner name
        owner.Name = ownerRequest.Name;

        await ownerRepository.UpdateOwnerAsync(ownerId, owner);

        return new OwnerResponse
        {
            Id = owner.Id,
            Name = owner.Name
        };
    }

    public async Task DeleteOwner(string ownerId)
    {
        var owner = await ownerRepository.GetOwnerByIdAsync(ownerId);
        if (owner == null)
            throw new NotFoundException($"Owner with id '{ownerId}' not found");

        await ownerRepository.DeleteOwnerAsync(ownerId);
    }

    

    
}