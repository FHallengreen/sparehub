namespace Service;

using Domain;
using Persistence;
using Shared;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class BoxMySqlService(SpareHubDbContext dbContext) : IBoxService
{
    public async Task<Box> CreateBox(BoxRequest boxRequest, int orderId)
    {
        // Check if the order exists
        var order = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
        {
            throw new Exception($"Order with ID {orderId} not found.");
        }

        // Create a new box
        var newBox = new Box
        {
            Length = boxRequest.Length,
            Width = boxRequest.Width,
            Height = boxRequest.Height,
            Weight = boxRequest.Weight
        };

        // Add the box to the database
        dbContext.Boxes.Add(newBox);
        await dbContext.SaveChangesAsync();

        // Add the box to the order using the join table
        dbContext.Entry(order).Collection(o => o.Boxes).Load(); // Load the navigation property
        order.Boxes.Add(newBox);
        await dbContext.SaveChangesAsync();

        return newBox;
    }

    public async Task<List<OrderBoxCollection>> GetBoxes(int orderId)
    {
        // Load boxes for the specified order via the join table
        var order = await dbContext.Orders
            .Include(o => o.Boxes) // Load boxes via navigation property
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

    public async Task DeleteBox(int orderId, int boxId)
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