using System.ComponentModel.DataAnnotations;
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
        var boxEntities = await dbContext.Boxes
            .Where(b => b.OrderId == int.Parse(orderId))
            .ToListAsync();

        var boxes = mapper.Map<List<Box>>(boxEntities);
        return boxes;
    }

    public async Task UpdateBoxesAsync(string orderId, List<Box> boxes)
    {
        var boxEntities = mapper.Map<List<BoxEntity>>(boxes);

        foreach (var boxEntity in boxEntities)
        {
            boxEntity.OrderId = int.Parse(orderId);
        }

        dbContext.Boxes.UpdateRange(boxEntities);

        await dbContext.SaveChangesAsync();
    }


    public Task DeleteBoxAsync(string orderId, string boxId)
    {
        dbContext.Boxes.Remove(new BoxEntity { Id = Guid.Parse(boxId) });
        return dbContext.SaveChangesAsync();
    }

    public async Task UpdateBoxAsync(string orderId, Box box)
    {
        var boxEntity = mapper.Map<BoxEntity>(box);
        boxEntity.OrderId = int.Parse(orderId);
        dbContext.Boxes.Update(boxEntity);
        await dbContext.SaveChangesAsync();

    }
}