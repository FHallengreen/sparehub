using AutoMapper;
using Domain.Models;
using MongoDB.Driver;
using Persistence.MongoDb;
using Repository.Interfaces;

namespace Repository.MongoDb;

public class DispatchMongoDbRepository : IDispatchRepository
{
    private readonly IMongoCollection<DispatchCollection> _collection;
    private readonly IMapper _mapper;

    public DispatchMongoDbRepository(IMongoCollection<DispatchCollection> collection, IMapper mapper)
    {
        _collection = collection;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Dispatch>> GetDispatchesAsync()
    {
        var dispatchCollection = await _collection.Find(dispatch => true).ToListAsync();
        return _mapper.Map<IEnumerable<Dispatch>>(dispatchCollection);
    }

    public async Task<Dispatch?> GetDispatchByIdAsync(string dispatchId)
    {
        var dispatch = await _collection.Find(dispatch => dispatch.Id == dispatchId).FirstOrDefaultAsync();
        return _mapper.Map<Dispatch>(dispatch);
    }

    public async Task<Dispatch> CreateDispatchAsync(Dispatch dispatch)
    {
        var dispatchCollection = _mapper.Map<DispatchCollection>(dispatch);
        await _collection.InsertOneAsync(dispatchCollection);
        return _mapper.Map<Dispatch>(dispatchCollection);
    }

    public async Task<Dispatch> UpdateDispatchAsync(Dispatch dispatch)
    {
        var dispatchCollection = _mapper.Map<DispatchCollection>(dispatch);
        var result = await _collection.ReplaceOneAsync(
            d => d.Id == dispatchCollection.Id,
            dispatchCollection,
            new ReplaceOptions { IsUpsert = true }
        );

        if (result.ModifiedCount == 0 && result.UpsertedId == null)
        {
            throw new Exception($"Update failed for Dispatch with ID: {dispatchCollection.Id}");
        }

        return dispatch;
    }

    public async Task DeleteDispatchAsync(string dispatchId)
    {
        var result = await _collection.DeleteOneAsync(dispatch => dispatch.Id == dispatchId);

        if (result.DeletedCount == 0)
        {
            throw new Exception($"Delete failed for Dispatch with ID: {dispatchId}");
        }
    }
}
