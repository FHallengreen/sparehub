using Shared.Order;

namespace Service.Interfaces;

public interface IBoxService
{
    Task<BoxResponse> CreateBox(BoxRequest boxRequest, string orderId);
    Task<List<BoxResponse>> GetBoxes(string orderId);
    Task UpdateOrderBoxes(string orderId, List<BoxRequest> boxes);
    Task DeleteBox(string orderId, string boxId);
}
