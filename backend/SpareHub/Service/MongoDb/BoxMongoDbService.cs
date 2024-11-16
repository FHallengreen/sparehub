/*
using Domain;
using Domain.MongoDb;
using MongoDB.Driver;
using Shared.Order;

namespace Service.MongoDb;

public class BoxMongoDbService(IMongoCollection<BoxOrderCollection> collection) : IBoxService
{
    public async Task<BoxResponse> CreateBox(BoxRequest boxRequest, int orderId)
    {
        var orderBox = await collection.Find(o => o.OrderId == orderId).FirstOrDefaultAsync();

        var newBox = new BoxCollection
        {
            Id = Guid.NewGuid(),
            Length = boxRequest.Length,
            Width = boxRequest.Width,
            Height = boxRequest.Height,
            Weight = boxRequest.Weight
        };

        if (orderBox == null)
        {
            orderBox = new BoxOrderCollection
            {
                OrderId = orderId,
                Boxes = [newBox]
            };

            await collection.InsertOneAsync(orderBox);
        }
        else
        {
            orderBox.Boxes.Add(newBox);
            await collection.ReplaceOneAsync(o => o.OrderId == orderId, orderBox);
        }

        return newBox;
    }

    public async Task<List<BoxOrderCollection>> GetBoxes(int orderId)
    {
        var orderBox = await collection.Find(o => o.OrderId == orderId).FirstOrDefaultAsync();

        if (orderBox == null)
        {
            return new List<BoxOrderCollection>();
        }

        return new List<BoxOrderCollection> { orderBox };
    }

    public async Task UpdateOrderBoxes(int orderId, List<BoxRequest> boxRequests)
    {
        var orderBox = await collection.Find(o => o.OrderId == orderId).FirstOrDefaultAsync();

        if (orderBox == null)
        {
            throw new Exception($"Order with ID {orderId} not found.");
        }

        var updatedBoxes = new List<BoxCollection>();

        foreach (var boxRequest in boxRequests)
        {
            if (boxRequest.BoxId == null || boxRequest.BoxId == Guid.Empty)
            {
                updatedBoxes.Add(new BoxCollection
                {
                    Id = Guid.NewGuid(),
                    Length = boxRequest.Length,
                    Width = boxRequest.Width,
                    Height = boxRequest.Height,
                    Weight = boxRequest.Weight
                });
            }
            else
            {
                var existingBox = orderBox.Boxes.FirstOrDefault(b => b.Id == boxRequest.BoxId);
                if (existingBox != null)
                {
                    existingBox.Length = boxRequest.Length;
                    existingBox.Width = boxRequest.Width;
                    existingBox.Height = boxRequest.Height;
                    existingBox.Weight = boxRequest.Weight;
                    updatedBoxes.Add(existingBox);
                }
                else
                {
                    updatedBoxes.Add(new BoxCollection
                    {
                        Id = boxRequest.BoxId.Value,
                        Length = boxRequest.Length,
                        Width = boxRequest.Width,
                        Height = boxRequest.Height,
                        Weight = boxRequest.Weight
                    });
                }
            }
        }

        orderBox.Boxes = updatedBoxes;

        await collection.ReplaceOneAsync(o => o.OrderId == orderId, orderBox);
    }

    public async Task DeleteBox(int orderId, Guid boxId)
    {
        var orderBox = await collection.Find(o => o.OrderId == orderId).FirstOrDefaultAsync();

        if (orderBox == null)
        {
            throw new Exception($"Order with ID {orderId} not found.");
        }

        var boxToRemove = orderBox.Boxes.FirstOrDefault(b => b.Id == boxId);

        if (boxToRemove == null)
        {
            throw new Exception($"Box with ID {boxId} not found in order {orderId}.");
        }

        orderBox.Boxes.Remove(boxToRemove);

        await collection.ReplaceOneAsync(o => o.OrderId == orderId, orderBox);
    }
}
*/
