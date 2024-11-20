using AutoMapper;
using Domain.Models;
using Domain.MySql;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Repository.Interfaces;

namespace Repository.MySql;

public class DispatchMySqlRepository(SpareHubDbContext dbContext, IMapper mapper) : IDispatchRepository
{
    public async Task<Dispatch> CreateDispatchAsync(Dispatch dispatch)
    {
        var dispatchEntity = mapper.Map<DispatchEntity>(dispatch);
        await dbContext.Dispatches.AddAsync(dispatchEntity);
        await dbContext.SaveChangesAsync();
        return mapper.Map<Dispatch>(dispatchEntity);
    }
    
    public async Task<Dispatch?> GetDispatchByIdAsync(string dispatchId)
    {
        if (!int.TryParse(dispatchId, out var id))
            return null;

        var dispatchEntity = await dbContext.Dispatches
            .Include(d => d.userEntity)
            .FirstOrDefaultAsync(d => d.Id == id);

        return dispatchEntity != null ? mapper.Map<Dispatch>(dispatchEntity) : null;
    }
    
    public async Task<IEnumerable<Dispatch>> GetDispatchesAsync()
    {
        var dispatchEntities = await dbContext.Dispatches
            .Include(d => d.userEntity)
            .ToListAsync();

        return mapper.Map<IEnumerable<Dispatch>>(dispatchEntities);
    }
    
    public async Task<Dispatch> UpdateDispatchAsync(Dispatch dispatch)
    {
        var dispatchEntity = mapper.Map<DispatchEntity>(dispatch);
        dbContext.Dispatches.Update(dispatchEntity);
        await dbContext.SaveChangesAsync();
        return mapper.Map<Dispatch>(dispatchEntity);
    }
    
    public async Task DeleteDispatchAsync(string dispatchId)
    {
        if (!int.TryParse(dispatchId, out var id))
            return;

        var dispatchEntity = await dbContext.Dispatches
            .FirstOrDefaultAsync(d => d.Id == id);

        if (dispatchEntity == null)
            return;

        dbContext.Dispatches.Remove(dispatchEntity);
        await dbContext.SaveChangesAsync();
    }
}