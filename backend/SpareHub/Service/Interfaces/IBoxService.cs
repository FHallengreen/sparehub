using Shared.DTOs.Order;
using Shared.Order;

namespace Service.Interfaces;

public interface IBoxService
{
    Task<BoxResponse> CreateBox(BoxRequest boxRequest, string orderId);
    Task<List<BoxResponse>> GetBoxes(string orderId);
    Task UpdateBoxes(string orderId, List<BoxRequest> boxes);
    Task UpdateBox(string orderId, string boxId, BoxRequest boxRequest);
    Task DeleteBox(string orderId, string boxId);
}
