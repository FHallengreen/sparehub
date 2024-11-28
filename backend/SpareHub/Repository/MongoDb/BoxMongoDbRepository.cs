using AutoMapper;
using Domain.Models;
using MongoDB.Driver;
using Persistence.MongoDb;
using Repository.Interfaces;

namespace Repository.MongoDb;

public class BoxMongoDbRepository(IMongoCollection<BoxCollection> collection, IMapper mapper) : IBoxRepository
{
    public async Task<Box> CreateBoxAsync(Box box)
    {
        var boxCollection = mapper.Map<BoxCollection>(box);

        await collection.InsertOneAsync(boxCollection);

        return mapper.Map<Box>(boxCollection);
    }

    public async Task<List<Box>> GetBoxesByOrderIdAsync(string orderId)
    {
        var filter = Builders<BoxCollection>.Filter.Eq(b => b.OrderId, orderId);
        var boxCollections = await collection.Find(filter).ToListAsync();
        return mapper.Map<List<Box>>(boxCollections);
    }

    public async Task UpdateBoxesAsync(string orderId, List<Box> boxes)
    {
        var boxEntities = mapper.Map<List<BoxCollection>>(boxes);

        foreach (var box in boxEntities)
        {
            box.OrderId = orderId;
        }

        foreach (var box in boxEntities)
        {
            var filter = Builders<BoxCollection>.Filter.Eq(b => b.Id, box.Id);
            await collection.ReplaceOneAsync(filter, box, new ReplaceOptions { IsUpsert = true });
        }
    }


    public async Task DeleteBoxAsync(string orderId, string boxId)
    {
        var filter = Builders<BoxCollection>.Filter.And(
            Builders<BoxCollection>.Filter.Eq(b => b.Id, boxId),
            Builders<BoxCollection>.Filter.Eq(b => b.OrderId, orderId)
        );

        await collection.DeleteOneAsync(filter);
    }

    public Task UpdateBoxAsync(string orderId, Box box)
    {
        throw new NotImplementedException();
    }
}