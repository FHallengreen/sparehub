using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.MySql;
using Persistence.MySql.SparehubDbContext;
using Repository.Interfaces;
using Shared.Exceptions;

namespace Repository.MySql;

public class OwnerMySqlRepository(SpareHubDbContext dbContext, IMapper mapper) : IOwnerRepository
{
    public async Task<List<Owner>> GetOwnersAsync()
    {
        var ownerEntities = await dbContext.Owners.ToListAsync();
        var owners = mapper.Map<List<Owner>>(ownerEntities);
        return owners;
    }

    public async Task<Owner> GetOwnerByIdAsync(string ownerId)
    {
        var ownerEntity = await dbContext.Owners
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == int.Parse(ownerId));
            
        var owner = mapper.Map<Owner>(ownerEntity);
        return owner;
    }


    public async Task<Owner> CreateOwnerAsync(Owner owner)
    {
        var ownerEntity = mapper.Map<OwnerEntity>(owner);
        await dbContext.Owners.AddAsync(ownerEntity);
        await dbContext.SaveChangesAsync();
        
        return mapper.Map<Owner>(ownerEntity);
    }

    public async Task UpdateOwnerAsync(string ownerId, Owner owner)
    {
        var ownerEntity = mapper.Map<OwnerEntity>(owner);
        dbContext.Owners.Update(ownerEntity);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteOwnerAsync(string ownerId)
    {
        var ownerEntity = await dbContext.Owners.FirstOrDefaultAsync(p => p.Id == int.Parse(ownerId));
        
        if (ownerEntity == null)
            throw new NotFoundException($"Owner with id '{ownerId}' not found");
        
        dbContext.Owners.Remove(ownerEntity);
        await dbContext.SaveChangesAsync();
    }
}