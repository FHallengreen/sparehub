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

    public Task<OrderBoxCollection> UpdateBoxes(BoxRequest boxRequest, int orderId)
    {
        throw new NotImplementedException();
    }

    public Task<OrderBoxCollection> UpdateBox(BoxRequest boxRequest, int orderId)
    {
        throw new NotImplementedException();
    }

    public void DeleteBox(int orderId)
    {
        throw new NotImplementedException();
    }
}
