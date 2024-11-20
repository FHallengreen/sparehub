using AutoMapper;
using Domain.Models;
using MongoDB.Driver;
using Persistence.MongoDb;
using Repository.Interfaces;

namespace Repository.MongoDb;

public class DispatchMongoDbRepository (IMongoCollection<DispatchCollection> collection, IMapper mapper) : IDispatchRepository
{
    public async Task<IEnumerable<Dispatch>> GetDispatchesAsync()
    {
        var dispatchCollection = await collection.Find(dispatch => true).ToListAsync();
        return mapper.Map<IEnumerable<Dispatch>>(dispatchCollection);
    }

    public async Task<Dispatch?> GetDispatchByIdAsync(string dispatchId)
    {
        var dispatch = await collection.Find(dispatch => dispatchId == dispatch.Id).FirstOrDefaultAsync();
        return mapper.Map<Dispatch>(dispatch);
    }

    public Task<Dispatch> CreateDispatchAsync(Dispatch dispatch)
    {
        throw new NotImplementedException();
    }

    public Task<Dispatch> UpdateDispatchAsync(Dispatch dispatch)
    {
        throw new NotImplementedException();
    }

    public Task DeleteDispatchAsync(string dispatchId)
    {
        throw new NotImplementedException();
    }
}