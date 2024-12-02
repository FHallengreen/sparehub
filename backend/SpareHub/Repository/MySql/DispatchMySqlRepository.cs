using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.MySql;
using Persistence.MySql.SparehubDbContext;
using Repository.Interfaces;

namespace Repository.MySql;

public class DispatchMySqlRepository : IDispatchRepository
{
    private readonly SpareHubDbContext _dbContext;
    private readonly IMapper _mapper;

    public DispatchMySqlRepository(SpareHubDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Dispatch> CreateDispatchAsync(Dispatch dispatch)
    {
        var dispatchEntity = _mapper.Map<DispatchEntity>(dispatch);
        await _dbContext.Dispatches.AddAsync(dispatchEntity);
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<Dispatch>(dispatchEntity);
    }
    
    public async Task<Dispatch?> GetDispatchByIdAsync(string dispatchId)
    {
        if (!int.TryParse(dispatchId, out var id))
            return null;

        var dispatchEntity = await _dbContext.Dispatches
            .Include(d => d.Orders)
            .FirstOrDefaultAsync(d => d.Id == id);

        return dispatchEntity != null ? _mapper.Map<Dispatch>(dispatchEntity) : null;
    }
    
    public async Task<IEnumerable<Dispatch>> GetDispatchesAsync()
    {
        var dispatchEntities = await _dbContext.Dispatches
            .Include(d => d.Orders)
            .ToListAsync();

        return _mapper.Map<IEnumerable<Dispatch>>(dispatchEntities);
    }
    
    public async Task<Dispatch> UpdateDispatchAsync(Dispatch dispatch)
    {
        var dispatchEntity = _mapper.Map<DispatchEntity>(dispatch);
        _dbContext.Dispatches.Update(dispatchEntity);
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<Dispatch>(dispatchEntity);
    }
    
    public async Task DeleteDispatchAsync(string dispatchId)
    {
        if (!int.TryParse(dispatchId, out var id))
            return;

        var dispatchEntity = await _dbContext.Dispatches
            .FirstOrDefaultAsync(d => d.Id == id);

        if (dispatchEntity == null)
            return;

        _dbContext.Dispatches.Remove(dispatchEntity);
        await _dbContext.SaveChangesAsync();
    }
}
