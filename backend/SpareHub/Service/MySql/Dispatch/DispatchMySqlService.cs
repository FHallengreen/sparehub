using Repository.Interfaces;
using Repository.MySql;
using Service.Interfaces;
using Shared.DTOs.Dispatch;
using Shared.Exceptions;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace Service.MySql.Dispatch;

public class DispatchMySqlService(DispatchMySqlRepository dispatchRepository) : IDispatchService
{
    public async Task<DispatchResponse> CreateDispatch(DispatchRequest dispatchRequest, string orderId)
    {
        if (string.IsNullOrWhiteSpace(orderId))
            throw new ValidationException("Order ID cannot be null or empty.");

        if (dispatchRequest == null)
            throw new ValidationException("Dispatch request cannot be null.");

        var dispatch = new Domain.Models.Dispatch
        {
            OriginType = dispatchRequest.OriginType,
            OriginId = dispatchRequest.OriginId,
            DestinationType = dispatchRequest.DestinationType,
            DestinationId = dispatchRequest.DestinationId,
            TransportModeType = dispatchRequest.TransportModeType,
            UserId = dispatchRequest.UserId
        };

        var createdDispatch = await dispatchRepository.CreateDispatchAsync(dispatch);

        return new DispatchResponse
        {
            Id = createdDispatch.Id,
            OriginType = createdDispatch.OriginType,
            OriginId = createdDispatch.OriginId,
            DestinationType = createdDispatch.DestinationType,
            DestinationId = createdDispatch.DestinationId,
            DispatchStatus = createdDispatch.DispatchStatus,
            TransportModeType = createdDispatch.TransportModeType,
            TrackingNumber = createdDispatch.TrackingNumber,
            DispatchDate = createdDispatch.DispatchDate,
            DeliveryDate = createdDispatch.DeliveryDate,
            UserId = createdDispatch.UserId,
            User = createdDispatch.User
        };
    }

    public async Task<DispatchResponse> GetDispatchById(string dispatchId)
    {
        var dispatch = await dispatchRepository.GetDispatchByIdAsync(dispatchId);

        if (dispatch == null)
            throw new NotFoundException("Dispatch not found.");

        return new DispatchResponse
        {
            Id = dispatch.Id,
            OriginType = dispatch.OriginType,
            OriginId = dispatch.OriginId,
            DestinationType = dispatch.DestinationType,
            DestinationId = dispatch.DestinationId,
            DispatchStatus = dispatch.DispatchStatus,
            TransportModeType = dispatch.TransportModeType,
            TrackingNumber = dispatch.TrackingNumber,
            DispatchDate = dispatch.DispatchDate,
            DeliveryDate = dispatch.DeliveryDate,
            UserId = dispatch.UserId,
            User = dispatch.User
        };
    }

    public async Task<IEnumerable<DispatchResponse>> GetDispatches()
    {
        var dispatches = await dispatchRepository.GetDispatchesAsync();

        if (dispatches == null || !dispatches.Any())
            throw new NotFoundException("No dispatches found.");

        return dispatches.Select(d => new DispatchResponse
        {
            Id = d.Id,
            OriginType = d.OriginType,
            OriginId = d.OriginId,
            DestinationType = d.DestinationType,
            DestinationId = d.DestinationId,
            DispatchStatus = d.DispatchStatus,
            TransportModeType = d.TransportModeType,
            TrackingNumber = d.TrackingNumber,
            DispatchDate = d.DispatchDate,
            DeliveryDate = d.DeliveryDate,
            UserId = d.UserId,
            User = d.User
        });
    }

    public async Task<DispatchResponse> UpdateDispatch(string dispatchId, DispatchRequest dispatchRequest)
    {
        if (string.IsNullOrWhiteSpace(dispatchId))
            throw new ValidationException("Dispatch ID cannot be null or empty.");

        if (dispatchRequest == null)
            throw new ValidationException("Dispatch request cannot be null.");

        var dispatch = new Domain.Models.Dispatch
        {
            Id = dispatchId,
            OriginType = dispatchRequest.OriginType,
            OriginId = dispatchRequest.OriginId,
            DestinationType = dispatchRequest.DestinationType,
            DestinationId = dispatchRequest.DestinationId,
            TransportModeType = dispatchRequest.TransportModeType,
            UserId = dispatchRequest.UserId
        };

        var updatedDispatch = await dispatchRepository.UpdateDispatchAsync(dispatch);

        return new DispatchResponse
        {
            Id = updatedDispatch.Id,
            OriginType = updatedDispatch.OriginType,
            OriginId = updatedDispatch.OriginId,
            DestinationType = updatedDispatch.DestinationType,
            DestinationId = updatedDispatch.DestinationId,
            DispatchStatus = updatedDispatch.DispatchStatus,
            TransportModeType = updatedDispatch.TransportModeType,
            TrackingNumber = updatedDispatch.TrackingNumber,
            DispatchDate = updatedDispatch.DispatchDate,
            DeliveryDate = updatedDispatch.DeliveryDate,
            UserId = updatedDispatch.UserId,
            User = updatedDispatch.User
        };
    }

    public async Task DeleteDispatch(string dispatchId)
    {
        if (string.IsNullOrWhiteSpace(dispatchId))
            throw new ValidationException("Dispatch ID cannot be null or empty.");

        await dispatchRepository.DeleteDispatchAsync(dispatchId);
    }
}