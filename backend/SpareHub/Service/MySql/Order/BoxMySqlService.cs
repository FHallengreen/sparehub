using Domain;
using Domain.MongoDb;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Shared.Order;

namespace Service.Order;

public class BoxMySqlService(SpareHubDbContext dbContext) : IBoxService
{
    public async Task<Box> CreateBox(BoxRequest boxRequest, int orderId)
    {
        var order = await dbContext.Orders
            .Include(o => o.Boxes)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
        {
            throw new Exception($"Order with ID {orderId} not found.");
        }

        var newBox = new Box
        {
            Id = Guid.NewGuid(),
            Length = boxRequest.Length,
            Width = boxRequest.Width,
            Height = boxRequest.Height,
            Weight = boxRequest.Weight
        };

        dbContext.Boxes.Add(newBox);
        order.Boxes.Add(newBox);

        await dbContext.SaveChangesAsync();
        return newBox;
    }


    public async Task<List<BoxOrderCollection>> GetBoxes(int orderId)
    {
        var order = await dbContext.Orders
            .Include(o => o.Boxes)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
        {
            throw new KeyNotFoundException("Order not found");
        }

        return
        [
            new BoxOrderCollection
            {
                OrderId = order.Id,
                Boxes = order.Boxes.ToList()
            }
        ];
    }


    public async Task UpdateOrderBoxes(int orderId, List<BoxRequest> boxRequests)
    {
        var order = await dbContext.Orders
            .Include(o => o.Boxes)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
        {
            throw new Exception($"Order with ID {orderId} not found.");
        }

        var existingBoxes = order.Boxes.ToList();

        foreach (var boxRequest in boxRequests.Where(b => b.BoxId != Guid.Empty))
        {
            var existingBox = existingBoxes.FirstOrDefault(b => b.Id == boxRequest.BoxId);
            if (existingBox != null)
            {
                existingBox.Length = boxRequest.Length;
                existingBox.Width = boxRequest.Width;
                existingBox.Height = boxRequest.Height;
                existingBox.Weight = boxRequest.Weight;
            }
        }

        foreach (var boxRequest in boxRequests.Where(b => b.BoxId == Guid.Empty))
        {
            var newBox = new Box
            {
                Id = Guid.NewGuid(),
                Length = boxRequest.Length,
                Width = boxRequest.Width,
                Height = boxRequest.Height,
                Weight = boxRequest.Weight
            };

            order.Boxes.Add(newBox);
        }

        foreach (var box in existingBoxes)
        {
            if (boxRequests.All(b => b.BoxId != box.Id))
            {
                dbContext.Boxes.Remove(box);
            }
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
