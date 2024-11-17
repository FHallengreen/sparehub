using System.ComponentModel.DataAnnotations;
using Domain.Models;
using Repository.Interfaces;
using Service.Interfaces;
using Shared.Exceptions;
using Shared.Order;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace Service.MySql.Order;

public class BoxMySqlService(IBoxRepository boxRepository) : IBoxService
{
    public async Task<BoxResponse> CreateBox(BoxRequest boxRequest, string orderId)
    {
        if (string.IsNullOrWhiteSpace(orderId))
            throw new ValidationException("Order ID cannot be null or empty.");

        if (boxRequest == null)
            throw new ValidationException("Box request cannot be null.");

        try
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

            return new BoxResponse
            {
                Id = createdBox.Id,
                OrderId = createdBox.OrderId,
                Length = createdBox.Length,
                Width = createdBox.Width,
                Height = createdBox.Height,
                Weight = createdBox.Weight
            };
        }
        catch (Exception ex)
        {
            throw new RepositoryException("Failed to create box.", ex);
        }
    }

    public async Task<List<BoxResponse>> GetBoxes(string orderId)
    {
        if (string.IsNullOrWhiteSpace(orderId))
            throw new ValidationException("Order ID cannot be null or empty.");
        
        try
        {
            var boxes = await boxRepository.GetBoxesByOrderIdAsync(orderId);

            if (boxes == null || !boxes.Any())
                throw new NotFoundException($"No boxes found for order ID '{orderId}'.");

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
        catch (Exception ex)
        {
            throw new RepositoryException("Failed to fetch boxes.", ex);
        }
    }

    public async Task UpdateOrderBoxes(string orderId, List<BoxRequest> boxRequests)
    {
        if (string.IsNullOrWhiteSpace(orderId))
            throw new ValidationException("Order ID cannot be null or empty.");

        try
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
        catch (Exception ex)
        {
            throw new RepositoryException("Failed to update boxes.", ex);
        }
    }

    public async Task DeleteBox(string orderId, string boxId)
    {
        if (string.IsNullOrWhiteSpace(orderId) || string.IsNullOrWhiteSpace(boxId))
            throw new ValidationException("Order ID and Box ID cannot be null or empty.");

        try
        {
            await boxRepository.DeleteBoxAsync(orderId, boxId);
        }
        catch (Exception ex)
        {
            throw new RepositoryException("Failed to delete box.", ex);
        }
    }
}

