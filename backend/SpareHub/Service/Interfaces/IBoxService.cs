using Domain;
using Domain.MongoDb;
using Service.Order;
using Shared;
using Shared.Order;

namespace Service;

public interface IBoxService
{
    public Task<Box> CreateBox(BoxRequest boxRequest, int orderId);
    public Task<List<BoxOrderCollection>> GetBoxes(int orderId);
    public Task UpdateOrderBoxes (int orderId, List<BoxRequest> boxes);
    public Task DeleteBox(int orderId, Guid boxId);
    
}
