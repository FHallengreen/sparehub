using AutoMapper;
using Domain.Models;
using Domain.MySql;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Repository.Interfaces;

namespace Repository.MySql;

public class BoxMySqlRepository(SpareHubDbContext dbContext, IMapper mapper) : IBoxRepository
{
    public async Task<Box> CreateBoxAsync(Box box)
    {
        var boxEntity = mapper.Map<BoxEntity>(box);
        dbContext.Boxes.Add(boxEntity);
        await dbContext.SaveChangesAsync();
        box.Id = boxEntity.Id.ToString();
        return box;
    }

    public async Task<List<Box>> GetBoxesByOrderIdAsync(string orderId)
    {
        if (!int.TryParse(orderId, out var id))
            return new List<Box>();

        var boxEntities = await dbContext.Boxes
            .Where(b => b.OrderId == id)
            .ToListAsync();

        var boxes = mapper.Map<List<Box>>(boxEntities);
        return boxes;
    }


    public async Task UpdateBoxesAsync(string orderId, List<Box> boxes)
    {
        if (!int.TryParse(orderId, out var id))
            return;

        var existingBoxes = await dbContext.Boxes
            .Where(b => b.OrderId == id)
            .ToListAsync();

        dbContext.Boxes.RemoveRange(existingBoxes);

        var boxEntities = mapper.Map<List<BoxEntity>>(boxes);
        foreach (var boxEntity in boxEntities)
        {
            boxEntity.OrderId = id;
        }

        dbContext.Boxes.AddRange(boxEntities);
        await dbContext.SaveChangesAsync();
    }

    public Task DeleteBoxAsync(string orderId, string boxId)
    {
        throw new NotImplementedException();
    }
}
