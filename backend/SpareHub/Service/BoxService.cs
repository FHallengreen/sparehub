using Domain;
using MongoDB.Driver;
using Shared;

namespace Service;

public class BoxService (IMongoCollection<OrderBoxCollection> collection) : IBoxService
{
    public async Task CreateBox(BoxRequest boxRequest, int orderId)
    {
        var orderBox = await collection.Find(o => o.OrderId == orderId).FirstOrDefaultAsync();

        if (orderBox == null)
        {
            orderBox = new OrderBoxCollection
            {
                OrderId = orderId,
                Boxes =
                [
                    new Box
                    {
                        Length = boxRequest.Length,
                        Width = boxRequest.Width,
                        Height = boxRequest.Height,
                        Weight = boxRequest.Weight
                    }
                ]
            };

            await collection.InsertOneAsync(orderBox);
        }
        else
        {
            orderBox.Boxes.Add(new Box
            {
                Length = boxRequest.Length,
                Width = boxRequest.Width,
                Height = boxRequest.Height,
                Weight = boxRequest.Weight
            });

            await collection.ReplaceOneAsync(o => o.OrderId == orderId, orderBox);
        }
    }



    public async Task<List<OrderBoxCollection>> GetBoxes(int orderId)
    {
        var boxes = await collection.Find(o => Equals(o.OrderId, orderId)).ToListAsync();
        return boxes;
    }

    public async Task UpdateOrderBoxes(int orderId, List<BoxRequest> boxes)
    {
        var existingOrderBoxes = await collection.Find(b => b.OrderId == orderId).FirstOrDefaultAsync();

        if (existingOrderBoxes != null)
        {
             existingOrderBoxes.Boxes = boxes.Select(box => new Box
            {
                Length = box.Length,
                Width = box.Width,
                Height = box.Height,
                Weight = box.Weight
            }).ToList();

            await collection.ReplaceOneAsync(b => b.OrderId == orderId, existingOrderBoxes);
        }
        else
        {
            await collection.InsertOneAsync(new OrderBoxCollection
            {
                OrderId = orderId,
                Boxes = boxes.Select(box => new Box
                {
                    Length = box.Length,
                    Width = box.Width,
                    Height = box.Height,
                    Weight = box.Weight
                }).ToList()
            });
        }
    }

    public void DeleteBox(int orderId)
    {
        throw new NotImplementedException();
    }
}
