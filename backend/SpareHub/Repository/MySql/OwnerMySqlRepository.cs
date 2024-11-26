using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Repository.Interfaces;

namespace Repository.MySql;

public class OwnerMySqlRepository(SpareHubDbContext dbContext, IMapper mapper) : IOwnerRepository
{
    public Task<List<Owner>> GetOwnersAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Owner> GetOwnerByIdAsync(string ownerId)
    {
        var ownerEntity = await dbContext.Owners
            .FirstOrDefaultAsync(o => o.Id == int.Parse(ownerId));
        
        if (ownerEntity == null)
            return null;
        
        return mapper.Map<Owner>(ownerEntity);
    }


    public Task<Owner> CreateOwnerAsync(Owner owner)
    {
        throw new NotImplementedException();
    }

    public Task UpdateOwnerAsync(string ownerId, Owner owner)
    {
        throw new NotImplementedException();
    }

    public Task DeleteOwnerAsync(string ownerId)
    {
        throw new NotImplementedException();
    }
}