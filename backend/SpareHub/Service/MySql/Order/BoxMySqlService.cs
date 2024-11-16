using Domain.Models;
using Repository.Interfaces;
using Service.Interfaces;
using Shared.Order;

namespace Service.MySql.Order;

public class BoxMySqlService(IBoxRepository boxRepository) : IBoxService
{
    public async Task<BoxResponse> CreateBox(BoxRequest boxRequest, string orderId)
    {
        var box = new Box
        {
            Id = boxRequest.Id ?? Guid.NewGuid().ToString(),
            OrderId = orderId,
            Length = boxRequest.Length,
            Width = boxRequest.Width,
            Height = boxRequest.Height,
            Weight = boxRequest.Weight
        };

        var createdBox = await boxRepository.CreateBoxAsync(box);

        var boxResponse = new BoxResponse
        {
            Id = createdBox.Id,
            OrderId = createdBox.OrderId,
            Length = createdBox.Length,
            Width = createdBox.Width,
            Height = createdBox.Height,
            Weight = createdBox.Weight
        };

        return boxResponse;
    }

    public async Task<List<BoxResponse>> GetBoxes(string orderId)
    {
        var boxes = await boxRepository.GetBoxesByOrderIdAsync(orderId);

        return boxes.Select(b => new BoxResponse
        {
            Id = b.Id,
            OrderId = b.OrderId,
            Length = b.Length,
            Width = b.Width,
            Height = b.Height,
            Weight = b.Weight
        }).ToList();
    }

    public async Task UpdateOrderBoxes(string orderId, List<BoxRequest> boxRequests)
    {
        var boxes = boxRequests.Select(b => new Box
        {
            Id = b.Id ?? Guid.NewGuid().ToString(),
            OrderId = orderId,
            Length = b.Length,
            Width = b.Width,
            Height = b.Height,
            Weight = b.Weight
        }).ToList();

        await boxRepository.UpdateBoxesAsync(orderId, boxes);
    }

    public async Task DeleteBox(string orderId, string boxId)
    {
        await boxRepository.DeleteBoxAsync(orderId, boxId);
    }
}
