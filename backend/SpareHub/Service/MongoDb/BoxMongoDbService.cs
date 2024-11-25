using AutoMapper;
using Domain;
using MongoDB.Bson;
using MongoDB.Driver;
using Persistence.MongoDb;
using Repository.MongoDb;
using Service.Interfaces;
using Shared.DTOs.Order;
using Shared.Exceptions;
using Shared.Order;

namespace Service.MongoDb;

public class BoxMongoDbService(IMongoCollection<BoxCollection> collection,
    BoxMongoDbRepository boxRepository, IMapper mapper) : IBoxService
{
    public async Task<BoxResponse> CreateBox(BoxRequest boxRequest, string orderId)
    {
        var boxRequestWithId = boxRequest with
        {
            Id = string.IsNullOrWhiteSpace(boxRequest.Id)
                ? ObjectId.GenerateNewId().ToString()
                : boxRequest.Id
        };

        var boxCollection = mapper.Map<BoxCollection>(boxRequestWithId);
        boxCollection.OrderId = orderId;

        await collection.InsertOneAsync(boxCollection);

        return mapper.Map<BoxResponse>(boxCollection);
    }

    public async Task<List<BoxResponse>> GetBoxes(string orderId)
    {
        var boxes = await boxRepository.GetBoxesByOrderIdAsync(orderId);

        if (boxes == null || boxes.Count == 0)
            throw new NotFoundException($"No boxes found for order ID '{orderId}'.");

        return mapper.Map<List<BoxResponse>>(boxes);
    }

    public Task UpdateBoxes(string orderId, List<BoxRequest> boxes)
    {
        throw new NotImplementedException();
    }

    public Task UpdateBox(string orderId, string boxId, BoxRequest boxRequest)
    {
        throw new NotImplementedException();
    }

    public Task DeleteBox(string orderId, string boxId)
    {
        throw new NotImplementedException();
    }
}