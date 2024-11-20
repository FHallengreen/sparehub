using Shared.DTOs.Dispatch;

namespace Service.Interfaces;

public interface IDispatchService
{
    Task<DispatchResponse> CreateDispatch(DispatchRequest dispatchRequest, string orderId);
    Task<DispatchResponse> GetDispatchById(string dispatchId);
    Task<IEnumerable<DispatchResponse>> GetDispatches();
    Task<DispatchResponse> UpdateDispatch(string dispatchId, DispatchRequest dispatchRequest);
    Task DeleteDispatch(string dispatchId);
}