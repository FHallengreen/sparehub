using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
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

        // Convert Orders to List for easier handling
        var orders = dispatchEntity.Orders.ToList();

        for (int i = 0; i < orders.Count; i++)
        {
            var order = orders[i];

            // Check if the OrderEntity is already tracked
            var trackedOrder = _dbContext.ChangeTracker.Entries<OrderEntity>()
                .FirstOrDefault(e => e.Entity.Id == order.Id);

            if (trackedOrder == null)
            {
                // Attach the OrderEntity if not tracked
                _dbContext.Attach(order);
            }
            else
            {
                // Reuse the tracked OrderEntity
                orders[i] = trackedOrder.Entity;
            }

            // Handle SupplierEntity if present
            if (order.Supplier != null)
            {
                var trackedSupplier = _dbContext.ChangeTracker.Entries<SupplierEntity>()
                    .FirstOrDefault(e => e.Entity.Id == order.Supplier.Id);

                if (trackedSupplier == null)
                {
                    // Attach SupplierEntity if not tracked
                    _dbContext.Attach(order.Supplier);
                }
                else
                {
                    // Reuse the tracked SupplierEntity
                    order.Supplier = trackedSupplier.Entity;
                }
            }

            // Handle VesselEntity if present
            if (order.Vessel != null)
            {
                var trackedVessel = _dbContext.ChangeTracker.Entries<VesselEntity>()
                    .FirstOrDefault(e => e.Entity.Id == order.Vessel.Id);

                if (trackedVessel == null)
                {
                    // Attach VesselEntity if not tracked
                    _dbContext.Attach(order.Vessel);
                }
                else
                {
                    // Reuse the tracked VesselEntity
                    order.Vessel = trackedVessel.Entity;
                }
            }
        }

        // Replace Orders collection
        dispatchEntity.Orders = orders;

        // Add the DispatchEntity
        _dbContext.Dispatches.Add(dispatchEntity);
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

    public async Task<IEnumerable<int>> GetSupplierIdsAsync()
    {
        return await _dbContext.Dispatches
            .Select(d => d.OriginId)
            .Distinct()
            .ToListAsync();
    }

    public async Task<IEnumerable<int>> GetWarehouseIdsAsync()
    {
        return await _dbContext.Dispatches
            .Select(d => d.OriginId)
            .Distinct()
            .ToListAsync();
    }
}