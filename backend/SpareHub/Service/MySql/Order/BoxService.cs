using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Models;
using Repository.Interfaces;
using Repository.MySql;
using Service.Interfaces;
using Shared.DTOs.Order;
using Shared.Exceptions;

namespace Service.MySql.Order;

public class BoxService(IBoxRepository boxRepository, IOrderRepository orderRepository)
    : IBoxService
{
    public async Task<BoxResponse> CreateBox(BoxRequest boxRequest, string orderId)
    {
        if (string.IsNullOrWhiteSpace(orderId))
            throw new ValidationException("Order ID cannot be null or empty.");

        var order = await orderRepository.GetOrderByIdAsync(orderId);
        if (order == null)
            throw new ValidationException($"Order ID '{orderId}' is invalid or does not exist.");


        var box = new Box
        {
            Id = Guid.NewGuid().ToString(),
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


    public async Task<List<BoxResponse>> GetBoxes(string orderId)
    {
        var boxes = await boxRepository.GetBoxesByOrderIdAsync(orderId);

        if (boxes == null || boxes.Count == 0)
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

    public async Task UpdateBoxes(string orderId, List<BoxRequest> boxRequests)
    {
        if (string.IsNullOrWhiteSpace(orderId))
            throw new ValidationException("Order ID cannot be null or empty.");

        if (boxRequests.Exists(boxRequest => string.IsNullOrWhiteSpace(boxRequest.Id)))
        {
            throw new ValidationException("Box ID is required for updating a box.");
        }

        var boxes = boxRequests.Select(b => new Box
        {
            Id = b.Id ?? throw new InvalidOperationException(),
            OrderId = orderId,
            Length = b.Length,
            Width = b.Width,
            Height = b.Height,
            Weight = b.Weight
        }).ToList();

        await boxRepository.UpdateBoxesAsync(orderId, boxes);
    }

    public Task UpdateBox(string orderId, string boxId, BoxRequest boxRequest)
    {
        if (string.IsNullOrWhiteSpace(orderId))
            throw new ValidationException("Order ID cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(boxId))
            throw new ValidationException("Box ID cannot be null or empty.");

        var box = new Box
        {
            Id = boxId,
            OrderId = orderId,
            Length = boxRequest.Length,
            Width = boxRequest.Width,
            Height = boxRequest.Height,
            Weight = boxRequest.Weight
        };

        return boxRepository.UpdateBoxAsync(orderId, box);
    }

    public async Task DeleteBox(string orderId, string boxId)
    {
        await boxRepository.DeleteBoxAsync(orderId, boxId);
    }
}