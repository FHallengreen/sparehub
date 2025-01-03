using AutoMapper;
using Domain.Models;
using MongoDB.Bson;
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
        var objectId = new ObjectId(orderId);
        var filter = Builders<BoxCollection>.Filter.Eq(b => b.OrderId, objectId);
        var boxCollections = await collection.Find(filter).ToListAsync();
        return mapper.Map<List<Box>>(boxCollections);
    }

    public async Task UpdateBoxesAsync(string orderId, List<Box> boxes)
    {
        var boxEntities = mapper.Map<List<BoxCollection>>(boxes);

        foreach (var box in boxEntities)
        {
            box.OrderId = ObjectId.Parse(orderId);
            var filter = Builders<BoxCollection>.Filter.Eq(b => b.Id, box.Id);
            await collection.ReplaceOneAsync(filter, box, new ReplaceOptions { IsUpsert = false });
        }
    }



    public async Task DeleteBoxAsync(string boxId)
    {
        var filter = Builders<BoxCollection>.Filter.Eq(b => b.Id, boxId);
        await collection.DeleteOneAsync(filter);
    }

    public async Task UpdateBoxAsync(string orderId, Box box)
    {
        var boxEntity = mapper.Map<BoxCollection>(box);
        boxEntity.OrderId = ObjectId.Parse(orderId);

        var filter = Builders<BoxCollection>.Filter.Eq(b => b.Id, box.Id);
        await collection.ReplaceOneAsync(filter, boxEntity, new ReplaceOptions { IsUpsert = false });
    }

}