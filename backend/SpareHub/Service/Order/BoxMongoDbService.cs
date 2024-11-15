using Domain;
using MongoDB.Driver;
using Shared;

namespace Service;

public class BoxMongoDbService(IMongoCollection<OrderBoxCollection> collection) : IBoxService
{
    public async Task<Box> CreateBox(BoxRequest boxRequest, int orderId)
    {
        var orderBox = await collection.Find(o => o.OrderId == orderId).FirstOrDefaultAsync();

        var newBox = new Box
        {
            Id = new Random().Next(1, int.MaxValue),
            Length = boxRequest.Length,
            Width = boxRequest.Width,
            Height = boxRequest.Height,
            Weight = boxRequest.Weight
        };

        if (orderBox == null)
        {
            orderBox = new OrderBoxCollection
            {
                OrderId = orderId,
                Boxes = new List<Box> { newBox }
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


    public async Task<List<OrderBoxCollection>> GetBoxes(int orderId)
    {
        var boxes = await collection.Find(o => Equals(o.OrderId, orderId)).ToListAsync();
        return boxes;
    }

public async Task UpdateOrderBoxes(int orderId, List<BoxRequest> boxRequests)
{
    var existingOrderBoxes = await collection.Find(b => b.OrderId == orderId).FirstOrDefaultAsync();

    if (existingOrderBoxes != null)
    {
        var updatedBoxes = new List<Box>();

        foreach (var boxRequest in boxRequests)
        {
            if (boxRequest.BoxId == 0)
            {
                int newBoxId = GenerateNewBoxId(existingOrderBoxes.Boxes);
                var newBox = new Box
                {
                    Id = newBoxId,
                    Length = boxRequest.Length,
                    Width = boxRequest.Width,
                    Height = boxRequest.Height,
                    Weight = boxRequest.Weight
                };
                updatedBoxes.Add(newBox);
            }
            else
            {
                var existingBox = existingOrderBoxes.Boxes.FirstOrDefault(b => b.Id == boxRequest.BoxId);
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
                    updatedBoxes.Add(new Box
                    {
                        Id = boxRequest.BoxId,
                        Length = boxRequest.Length,
                        Width = boxRequest.Width,
                        Height = boxRequest.Height,
                        Weight = boxRequest.Weight
                    });
                }
            }
        }

        existingOrderBoxes.Boxes = updatedBoxes;

        await collection.ReplaceOneAsync(b => b.OrderId == orderId, existingOrderBoxes);
    }
    else
    {
        var boxesToAdd = boxRequests.Select(boxRequest => new Box
        {
            Id = boxRequest.BoxId == 0 ? 1 : boxRequest.BoxId,
            Length = boxRequest.Length,
            Width = boxRequest.Width,
            Height = boxRequest.Height,
            Weight = boxRequest.Weight
        }).ToList();

        await collection.InsertOneAsync(new OrderBoxCollection
        {
            OrderId = orderId,
            Boxes = boxesToAdd
        });
    }
}


private int GenerateNewBoxId(List<Box> existingBoxes)
{
    if (existingBoxes == null || !existingBoxes.Any())
        return 1;
    else
        return existingBoxes.Max(b => b.Id) + 1;
}




    public async Task DeleteBox(int orderId, int boxId)
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