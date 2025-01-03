using Shared.DTOs.Order;

namespace Service.Interfaces;

public interface ITrackingService
{
    Task<TrackingResponse> GetTrackingStatusAsync(string trackingNumber);
}