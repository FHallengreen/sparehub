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
        box.Id = boxCollection.Id;

        return box;
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

            var filter = Builders<BoxCollection>.Filter.And(
                Builders<BoxCollection>.Filter.Eq(b => b.Id, box.Id),
                Builders<BoxCollection>.Filter.Eq(b => b.OrderId, orderId)
            );

            await collection.ReplaceOneAsync(filter, box, new ReplaceOptions { IsUpsert = false });
        }
    }


    public async Task DeleteBoxAsync(string orderId, string boxId)
    {
        var filter = Builders<BoxCollection>.Filter.And(
            Builders<BoxCollection>.Filter.Eq(b => b.Id, boxId),
            Builders<BoxCollection>.Filter.Eq(b => b.OrderId, orderId)
        );

        var result = await collection.DeleteOneAsync(filter);

        if (result.DeletedCount == 0)
        {
            throw new InvalidOperationException($"No box found with Id '{boxId}' and OrderId '{orderId}'.");
        }
    }

    public Task UpdateBoxAsync(string orderId, Box box)
    {
        throw new NotImplementedException();
    }
}