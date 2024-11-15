using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Shared.Order;

namespace Service.Order;

public class BoxMySqlService(SpareHubDbContext dbContext) : IBoxService
{
    public async Task<Box> CreateBox(BoxRequest boxRequest, int orderId)
    {
        var order = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
        {
            throw new Exception($"Order with ID {orderId} not found.");
        }

        var newBox = new Box
        {
            Id = boxRequest.BoxId == Guid.Empty
                ? Guid.NewGuid()
                : boxRequest.BoxId,
            Length = boxRequest.Length,
            Width = boxRequest.Width,
            Height = boxRequest.Height,
            Weight = boxRequest.Weight
        };

        dbContext.Boxes.Add(newBox);
        await dbContext.SaveChangesAsync();

        dbContext.Entry(order).Collection(o => o.Boxes).Load();
        order.Boxes.Add(newBox);
        await dbContext.SaveChangesAsync();

        return newBox;
    }

    public async Task<List<OrderBoxCollection>> GetBoxes(int orderId)
    {
        var order = await dbContext.Orders
            .Include(o => o.Boxes)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
        {
            throw new Exception($"Order with ID {orderId} not found.");
        }

        return new List<OrderBoxCollection>
        {
            new OrderBoxCollection
            {
                OrderId = orderId,
                Boxes = order.Boxes.ToList()
            }
        };
    }

    public async Task UpdateOrderBoxes(int orderId, List<BoxRequest> boxRequests)
    {
        var order = await dbContext.Orders.Include(o => o.Boxes).FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
        {
            throw new Exception($"Order with ID {orderId} not found.");
        }

        // Clear existing boxes
        dbContext.Entry(order).Collection(o => o.Boxes).Load();
        order.Boxes.Clear();

        // Add new boxes
        foreach (var boxRequest in boxRequests)
        {
            var newBox = new Box
            {
                Id = boxRequest.BoxId == Guid.Empty
                    ? Guid.NewGuid()
                    : boxRequest.BoxId, // Generate new GUID if BoxId is empty
                Length = boxRequest.Length,
                Width = boxRequest.Width,
                Height = boxRequest.Height,
                Weight = boxRequest.Weight
            };

            dbContext.Boxes.Add(newBox);
            order.Boxes.Add(newBox);
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteBox(int orderId, Guid boxId)
    {
        var order = await dbContext.Orders.Include(o => o.Boxes).FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
        {
            throw new Exception($"Order with ID {orderId} not found.");
        }

        var boxToRemove = order.Boxes.FirstOrDefault(b => b.Id == boxId);

        if (boxToRemove == null)
        {
            throw new Exception($"Box with ID {boxId} not found in order {orderId}.");
        }

        order.Boxes.Remove(boxToRemove);
        dbContext.Boxes.Remove(boxToRemove);

        await dbContext.SaveChangesAsync();
    }
}